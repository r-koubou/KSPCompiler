#nullable disable

using System;
using System.Globalization;

using Antlr4.Runtime;

using KSPCompiler.Domain.Ast;
using KSPCompiler.Domain.Ast.Expressions;
using KSPCompiler.Externals.Parser.Antlr.Translators.Extensions;

namespace KSPCompiler.Externals.Parser.Antlr.Translators
{
    // Expression node generation implementation
    public partial class CSTConverterVisitor
    {
        private TNode SetupExpressionNode<TNode>( TNode dest )
            where TNode : AstExpressionSyntaxNode
        {
            if( dest.Left != null )
            {
                dest.Left.Parent = dest;
            }

            if( dest.Right != null )
            {
                dest.Right.Parent = dest;
            }

            return dest;
        }

        private TNode VisitExpressionNodeImpl<TNode>(
            ParserRuleContext context,
            ParserRuleContext left,
            ParserRuleContext right )
            where TNode : AstExpressionSyntaxNode, new()
        {
            var node = new TNode();

            node.Import( context );
            node.Left  = left?.Accept( this ) as AstExpressionSyntaxNode;
            node.Right = right?.Accept( this ) as AstExpressionSyntaxNode;

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
                var node = new AstSymbolExpression();
                node.Import( context );
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
                var node = new AstIntLiteral( v );
                node.Import( context );
                return node;
            }
            // real value
            else if( realLiteral != null )
            {
                var node = new AstRealLiteral( double.Parse( realLiteral.GetText() ) );
                node.Import( context );
                return node;
            }
            // string value
            else if( stringLiteral != null )
            {
                var node = new AstStringLiteral( stringLiteral.GetText() );
                node.Import( context );
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
                var node = VisitExpressionNodeImpl<AstCallExpression>( context, callExpression, callArguments );

                // when given no arguments
                node.Right ??= new AstExpressionList( node );
                node.Import( context );

                return node;
            }
            #endregion
            //
            // 配列要素アクセス
            // postfixExpression LBRACKET expression RBRACKET
            //
            #region Array index
            else if( arrayExpression != null )
            {
                return VisitExpressionNodeImpl<AstArrayElementExpression>( context, arrayExpression, arrayIndexExpression );
            }
            #endregion

            throw new ArgumentException( $"Unknown context:{context.GetText()}" );
        }

        public override AstNode VisitExpressionList(KSPParser.ExpressionListContext context)
        {
            var node = new AstExpressionList();
            node.Import( context );

            VisitExpressionListRecursive( context, node );
            return node;
        }

        private void VisitExpressionListRecursive(
            KSPParser.ExpressionListContext context,
            AstExpressionList dest
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
                   expression.Accept( this ) as AstExpressionSyntaxNode
                );
            }
            // expression
            else
            {
                dest.Expressions.Add(
                   expression.Accept( this ) as AstExpressionSyntaxNode
                );
            }
        }

        public override AstNode VisitAssignmentExpression( KSPParser.AssignmentExpressionContext context )
        {
            var operatorToken = context.assignmentOperator().opr.Type;
            AstAssignmentExpression.OperatorType operatorType;

            switch( operatorToken )
            {
                case KSPParser.ASSIGN:
                    operatorType = AstAssignmentExpression.OperatorType.Assign;
                    break;
                default:
                    throw new NotSupportedException( $"Token ID: {operatorToken} is not supported" );
            }

            var node = VisitExpressionNodeImpl<AstAssignmentExpression>(
                context,
                context.postfixExpression(),
                context.expression()
            );
            node.Operator = operatorType;

            return node;
        }

        public override AstNode VisitAssignmentExpressionList(KSPParser.AssignmentExpressionListContext context)
        {
            var node = new AstAssignmentExpressionList();
            node.Import( context );

            VisitAssignmentExpressionListRecursive( context, node );
            return node;
        }

        private void VisitAssignmentExpressionListRecursive(
            KSPParser.AssignmentExpressionListContext context,
            AstAssignmentExpressionList dest
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
                    expression.Accept( this ) as AstAssignmentExpression
                );
            }
            //
            // assignmentExpression
            //
            else
            {
                dest.Expressions.Add(
                    expression.Accept( this ) as AstAssignmentExpression
                );
            }
        }
    }
}
