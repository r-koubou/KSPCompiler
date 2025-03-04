using Antlr4.Runtime;

using KSPCompiler.Infrastructures.Parser.Antlr;
using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Features.Compilation.Infrastructures.Parser.Antlr.Translators
{
    // implementation of binary operator node generation
    public partial class CstConverterVisitor
    {
        private AstNode VisitBinaryExpressionNodeImpl<TNode>(
            ParserRuleContext context,
            ParserRuleContext? nested,
            ParserRuleContext? left,
            ParserRuleContext? right )
            where TNode : AstExpressionNode, new()
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
            return VisitBinaryExpressionNodeImpl<AstStringConcatenateExpressionNode>(
                context,
                context.nested,
                context.left,
                context.right
            );
        }

        public override AstNode VisitLogicalOrExpression( KSPParser.LogicalOrExpressionContext context )
        {
            return VisitBinaryExpressionNodeImpl<AstLogicalOrExpressionNode>(
                context,
                context.nested,
                context.left,
                context.right
            );
        }

        public override AstNode VisitLogicalAndExpression( KSPParser.LogicalAndExpressionContext context )
        {
            return VisitBinaryExpressionNodeImpl<AstLogicalAndExpressionNode>(
                context,
                context.nested,
                context.left,
                context.right
            );
        }

        public override AstNode VisitBitwiseOrExpression( KSPParser.BitwiseOrExpressionContext context )
        {
            return VisitBinaryExpressionNodeImpl<AstBitwiseOrExpressionNode>(
                context,
                context.nested,
                context.left,
                context.right
            );
        }

        public override AstNode VisitLogicalXorExpression( KSPParser.LogicalXorExpressionContext context )
        {
            return VisitBinaryExpressionNodeImpl<AstLogicalXorExpressionNode>(
                context,
                context.nested,
                context.left,
                context.right
            );
        }

        public override AstNode VisitBitwiseAndExpression( KSPParser.BitwiseAndExpressionContext context )
        {
            return VisitBinaryExpressionNodeImpl<AstBitwiseAndExpressionNode>(
                context,
                context.nested,
                context.left,
                context.right
            );
        }

        public override AstNode VisitBitwiseXorExpression( KSPParser.BitwiseXorExpressionContext context )
        {
            return VisitBinaryExpressionNodeImpl<AstBitwiseXorExpressionNode>(
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

            if( context.opr == null )
            {
                return NullAstExpressionNode.Instance;
            }

            AstExpressionNode node = context.opr.Type switch
            {
                KSPLexer.BOOL_EQ =>
                    VisitExpressionNodeImpl<AstEqualExpressionNode>( context, context.left, context.right ),
                KSPLexer.BOOL_NE =>
                    VisitExpressionNodeImpl<AstNotEqualExpressionNode>( context, context.left, context.right ),
                _ =>
                    NullAstExpressionNode.Instance
            };
            return node;
        }

        public override AstNode VisitRelationalExpression( KSPParser.RelationalExpressionContext context )
        {
            if( context.nested != null )
            {
                return context.nested.Accept( this );
            }

            if( context.opr == null )
            {
                return NullAstExpressionNode.Instance;
            }

            AstExpressionNode node = context.opr.Type switch
            {
                KSPLexer.BOOL_GT =>
                    VisitExpressionNodeImpl<AstGreaterThanExpressionNode>( context, context.left, context.right ),
                KSPLexer.BOOL_LT =>
                    VisitExpressionNodeImpl<AstLessThanExpressionNode>( context, context.left, context.right ),
                KSPLexer.BOOL_GE =>
                    VisitExpressionNodeImpl<AstGreaterEqualExpressionNode>( context, context.left, context.right ),
                KSPLexer.BOOL_LE =>
                    VisitExpressionNodeImpl<AstLessEqualExpressionNode>( context, context.left, context.right ),
                _ =>
                    NullAstExpressionNode.Instance
            };
            return node;
        }

        public override AstNode VisitAdditiveExpression( KSPParser.AdditiveExpressionContext context )
        {
            if( context.nested != null )
            {
                return context.nested.Accept( this );
            }

            if( context.opr == null )
            {
                return NullAstExpressionNode.Instance;
            }

            AstExpressionNode node = context.opr.Type switch
            {
                KSPLexer.PLUS =>
                    VisitExpressionNodeImpl<AstAdditionExpressionNode>( context, context.left, context.right ),
                KSPLexer.MINUS =>
                    VisitExpressionNodeImpl<AstSubtractionExpressionNode>( context, context.left, context.right ),
                _ =>
                    NullAstExpressionNode.Instance
            };
            return node;
        }

        public override AstNode VisitMultiplicativeExpression( KSPParser.MultiplicativeExpressionContext context )
        {
            if( context.nested != null )
            {
                return context.nested.Accept( this );
            }

            if( context.opr == null )
            {
                return NullAstExpressionNode.Instance;
            }

            AstExpressionNode node = context.opr.Type switch
            {
                KSPLexer.MUL =>
                    VisitExpressionNodeImpl<AstMultiplyingExpressionNode>( context, context.left, context.right ),
                KSPLexer.DIV =>
                    VisitExpressionNodeImpl<AstDivisionExpressionNode>( context, context.left, context.right ),
                KSPLexer.MOD =>
                    VisitExpressionNodeImpl<AstModuloExpressionNode>( context, context.left, context.right ),
                _ =>
                    NullAstExpressionNode.Instance
            };
            return node;
        }
    }
}
