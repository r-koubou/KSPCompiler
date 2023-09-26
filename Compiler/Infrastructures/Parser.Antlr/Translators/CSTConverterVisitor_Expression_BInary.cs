using System;

using Antlr4.Runtime;

using KSPCompiler.Domain.Ast.Node;
using KSPCompiler.Domain.Ast.Node.Expressions;

namespace KSPCompiler.Infrastructures.Parser.Antlr.Translators
{
    // implementation of binary operator node generation
    public partial class CSTConverterVisitor
    {
        private AstNode VisitBinaryExpressionNodeImpl<TNode>(
            ParserRuleContext context,
            ParserRuleContext? nested,
            ParserRuleContext? left,
            ParserRuleContext? right )
            where TNode : AstExpressionSyntaxNode, new()
        {
            if( nested != null )
            {
                return nested.Accept( this );
            }

            return VisitExpressionNodeImpl<TNode>(
                context,
                left,
                right
            );
        }

        public override AstNode VisitStringConcatenateExpression( KSPParser.StringConcatenateExpressionContext context )
        {
            return VisitBinaryExpressionNodeImpl<AstStringConcatenateExpression>(
                context,
                context.nested,
                context.left,
                context.right
            );
        }

        public override AstNode VisitLogicalOrExpression( KSPParser.LogicalOrExpressionContext context )
        {
            return VisitBinaryExpressionNodeImpl<AstLogicalOrExpression>(
                context,
                context.nested,
                context.left,
                context.right
            );
        }

        public override AstNode VisitLogicalAndExpression( KSPParser.LogicalAndExpressionContext context )
        {
            return VisitBinaryExpressionNodeImpl<AstLogicalAndExpression>(
                context,
                context.nested,
                context.left,
                context.right
            );
        }

        public override AstNode VisitBitwiseOrExpression( KSPParser.BitwiseOrExpressionContext context )
        {
            return VisitBinaryExpressionNodeImpl<AstBitwiseOrExpression>(
                context,
                context.nested,
                context.left,
                context.right
            );
        }

        public override AstNode VisitBitwiseAndExpression( KSPParser.BitwiseAndExpressionContext context )
        {
            return VisitBinaryExpressionNodeImpl<AstBitwiseAndExpression>(
                context,
                context.nested,
                context.left,
                context.right
            );
        }

        public override AstNode VisitEqualityExpression( KSPParser.EqualityExpressionContext context )
        {
            if( context.nested != null )
            {
                return context.nested.Accept( this );
            }

            AstExpressionSyntaxNode node = context.opr.Type switch
            {
                KSPLexer.BOOL_EQ =>
                    VisitExpressionNodeImpl<AstEqualExpression>( context, context.left, context.right ),
                KSPLexer.BOOL_NE =>
                    VisitExpressionNodeImpl<AstNotEqualExpression>( context, context.left, context.right ),
                _ =>
                    throw new ArgumentException( $"context.opr.Type is {context.opr.Text}" ),
            };
            return node;
        }

        public override AstNode VisitRelationalExpression( KSPParser.RelationalExpressionContext context )
        {
            if( context.nested != null )
            {
                return context.nested.Accept( this );
            }

            AstExpressionSyntaxNode node = context.opr.Type switch
            {
                KSPLexer.BOOL_GT =>
                    VisitExpressionNodeImpl<AstGreaterThanExpression>( context, context.left, context.right ),
                KSPLexer.BOOL_LT =>
                    VisitExpressionNodeImpl<AstLessThanExpression>( context, context.left, context.right ),
                KSPLexer.BOOL_GE =>
                    VisitExpressionNodeImpl<AstGreaterEqualExpression>( context, context.left, context.right ),
                KSPLexer.BOOL_LE =>
                    VisitExpressionNodeImpl<AstLessEqualExpression>( context, context.left, context.right ),
                _ =>
                    throw new ArgumentException( $"context.opr.Type is {context.opr.Text}" ),
            };
            return node;
        }

        public override AstNode VisitAdditiveExpression( KSPParser.AdditiveExpressionContext context )
        {
            if( context.nested != null )
            {
                return context.nested.Accept( this );
            }

            AstExpressionSyntaxNode node = context.opr.Type switch
            {
                KSPLexer.PLUS =>
                    VisitExpressionNodeImpl<AstAdditionExpression>( context, context.left, context.right ),
                KSPLexer.MINUS =>
                    VisitExpressionNodeImpl<AstSubtractionExpression>( context, context.left, context.right ),
                _ =>
                    throw new ArgumentException( $"context.opr.Type is {context.opr.Text}" ),
            };
            return node;
        }

        public override AstNode VisitMultiplicativeExpression( KSPParser.MultiplicativeExpressionContext context )
        {
            if( context.nested != null )
            {
                return context.nested.Accept( this );
            }

            AstExpressionSyntaxNode node = context.opr.Type switch
            {
                KSPLexer.MUL =>
                    VisitExpressionNodeImpl<AstMultiplyingExpression>( context, context.left, context.right ),
                KSPLexer.DIV =>
                    VisitExpressionNodeImpl<AstDivisionExpression>( context, context.left, context.right ),
                KSPLexer.MOD =>
                    VisitExpressionNodeImpl<AstModuloExpression>( context, context.left, context.right ),
                _ =>
                    throw new ArgumentException( $"context.opr.Type is {context.opr.Text}" ),
            };
            return node;
        }
    }
}
