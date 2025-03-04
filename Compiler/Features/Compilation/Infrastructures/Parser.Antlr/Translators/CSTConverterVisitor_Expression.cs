using System.Globalization;

using Antlr4.Runtime;

using KSPCompiler.Features.Compilation.Infrastructures.Parser.Antlr.Translators.Extensions;
using KSPCompiler.Infrastructures.Parser.Antlr;
using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Shared.Domain.Ast.Nodes.Extensions;

namespace KSPCompiler.Features.Compilation.Infrastructures.Parser.Antlr.Translators
{
    // Expression node generation implementation
    public partial class CstConverterVisitor
    {
        private TNode SetupExpressionNode<TNode>( TNode dest )
            where TNode : AstExpressionNode
        {
            if( dest.Left.IsNotNull() )
            {
                dest.Left.Parent = dest;
            }

            if( dest.Right.IsNotNull() )
            {
                dest.Right.Parent = dest;
            }

            return dest;
        }

        private TNode VisitExpressionNodeImpl<TNode>(
            ParserRuleContext context,
            ParserRuleContext? left,
            ParserRuleContext? right )
            where TNode : AstExpressionNode, new()
        {
            var node = new TNode();

            node.Import( tokenStream, context );

            if( left != null )
            {
                node.Left = left.Accept( this ) as AstExpressionNode
                            ?? NullAstExpressionNode.Instance;
            }

            if( right != null )
            {
                node.Right = right.Accept( this ) as AstExpressionNode
                            ?? NullAstExpressionNode.Instance;
            }

            return SetupExpressionNode( node );
        }

        public override AstNode VisitExpression( KSPParser.ExpressionContext context )
        {
            return context.stringConcatenateExpression().Accept( this );
        }

        public override AstNode VisitPrimaryExpression(KSPParser.PrimaryExpressionContext context)
        {
            return VisitPrimaryExpressionRecursive( context );
        }

        private AstNode VisitPrimaryExpressionRecursive( KSPParser.PrimaryExpressionContext context )
        {
            var multiLine       = context.MULTI_LINE_DELIMITER();
            var identifier      = context.IDENTIFIER();
            var intLiteral      = context.INTEGER_LITERAL();
            var realLiteral     = context.REAL_LITERAL();
            var stringLiteral   = context.STRING_LITERAL();
            var expression      = context.expression();

            //
            // MULTI_LINE_DELIMITER
            //
            if( multiLine != null )
            {
                return VisitPrimaryExpressionRecursive( context.primaryExpression() );
            }
            //
            // IDENTIFIER
            //
            #region Identifier
            if( identifier != null )
            {
                var node = new AstSymbolExpressionNode();
                node.Import( tokenStream, context );
                node.Name = identifier.GetText();
                return node;
            }
            #endregion

            #region Literal
            //
            // (INTEGER_LITERAL | REAL_LITERAL | STRING_LITERAL)
            //
            if( intLiteral != null )
            {
                int v;
                string digits = intLiteral.GetText().ToLower();

                if( digits.StartsWith( "9" ) && digits.EndsWith( "h" ) )
                {
                    // hex
                    // -> 9[*******]h
                    digits = digits[ 1..^1 ];
                    v = int.Parse( digits, NumberStyles.AllowHexSpecifier );
                }
                else
                {
                    // decimal
                    v = int.Parse( digits );
                }
                var node = new AstIntLiteralNode( v );
                node.Import( tokenStream, context );
                return node;
            }
            // real value
            else if( realLiteral != null )
            {
                var node = new AstRealLiteralNode( double.Parse( realLiteral.GetText() ) );
                node.Import( tokenStream, context );
                return node;
            }
            // string value
            else if( stringLiteral != null )
            {
                var value = stringLiteral.GetText();

                // スクリプト上の最初と末尾のダブルクォートを取り除いてC#の文字列として扱う
                // パーサから得る ksp上の文字列リテラル: "\"abc\"" を C#上では string 型 "abc" にしたい
                value = value[ 1..^1 ];

                var node = new AstStringLiteralNode( value );
                node.Import( tokenStream, context );
                return node;
            }
            #endregion Literal

            //
            // LPARENT expression RPARENT
            //
            #region Expression
            return VisitChildren( expression );
            #endregion
        }

        public override AstNode VisitPostfixExpression( KSPParser.PostfixExpressionContext context )
        {
            var expression = context.expr;
            var callExpression = context.callExpr;
            var callArguments = context.callArgs;
            var arrayExpression = context.arrayExpr;
            var arrayIndexExpression = context.arrayIndexExpr;

            //
            // primaryExpression
            //
            #region PrimaryExpression
            if( expression != null )
            {
                return expression.Accept( this );
            }
            #endregion
            //
            // KSPコマンド・Cスタイル関数相当の呼び出し
            // postfixExpression LPARENT expressionList? RPARENT
            //
            #region Call
            else if( callExpression != null )
            {
                return VisitCallCommand( context, callExpression, callArguments );
            }
            #endregion
            //
            // 配列要素アクセス
            // postfixExpression LBRACKET expression RBRACKET
            //
            #region Array index
            else if( arrayExpression != null )
            {
                return VisitExpressionNodeImpl<AstArrayElementExpressionNode>( context, arrayExpression, arrayIndexExpression );
            }
            #endregion

            return NullAstExpressionNode.Instance;
        }

        public override AstNode VisitExpressionList(KSPParser.ExpressionListContext context)
        {
            var node = new AstExpressionListNode();
            node.Import( tokenStream, context );

            VisitExpressionListRecursive( context, node );
            return node;
        }

        private void VisitExpressionListRecursive(
            KSPParser.ExpressionListContext context,
            AstExpressionListNode dest
        )
        {
            var expressionList = context.expressionList();
            var expression     = context.expression();

            //
            // expressionList COMMA expression
            //
            if( expressionList != null )
            {
                VisitExpressionListRecursive( expressionList, dest );
                dest.Expressions.Add(
                    expression.Accept( this ) as AstExpressionNode
                    ?? NullAstExpressionNode.Instance
                );
            }
            // expression
            else
            {
                dest.Expressions.Add(
                    expression.Accept( this ) as AstExpressionNode
                    ?? NullAstExpressionNode.Instance
                );
            }
        }

        public override AstNode VisitAssignmentExpression( KSPParser.AssignmentExpressionContext context )
        {
            var operatorType = AstAssignmentExpressionNode.OperatorType.Assign;

            // TODO 複数の代入演算子をサポートする場合
            // var operatorToken = context.assignmentOperator().opr.Type;
            //
            // switch( operatorToken )
            // {
            //     case KSPParser.ASSIGN:
            //         operatorType = AstAssignmentExpressionNode.OperatorType.Assign;
            //         break;
            //     default:
            //         break;
            // }

            var node = VisitExpressionNodeImpl<AstAssignmentExpressionNode>(
                context,
                context.postfixExpression(),
                context.expression()
            );
            node.Operator = operatorType;

            return node;
        }

        public override AstNode VisitAssignmentExpressionList(KSPParser.AssignmentExpressionListContext context)
        {
            var node = new AstAssignmentExpressionListNode();
            node.Import( tokenStream, context );

            VisitAssignmentExpressionListRecursive( context, node );
            return node;
        }

        private void VisitAssignmentExpressionListRecursive(
            KSPParser.AssignmentExpressionListContext context,
            AstAssignmentExpressionListNode dest
        )
        {
            var assignmentList = context.assignmentExpressionList();
            var expression     = context.assignmentExpression();

            //
            // assignmentExpressionList COMMA assignmentExpression
            //
            if( assignmentList != null )
            {
                VisitAssignmentExpressionListRecursive( assignmentList, dest );
                dest.Expressions.Add(
                    (AstAssignmentExpressionNode)expression.Accept( this )
                );
            }
            //
            // assignmentExpression
            //
            else
            {
                dest.Expressions.Add(
                    (AstAssignmentExpressionNode)expression.Accept( this )
                );
            }
        }
    }
}
