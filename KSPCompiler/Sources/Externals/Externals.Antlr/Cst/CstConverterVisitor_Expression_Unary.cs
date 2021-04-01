using Antlr4.Runtime;

using KSPCompiler.Domain.Ast;
using KSPCompiler.Domain.Ast.Expressions;
using KSPCompiler.Externals.Antlr.Generated;

namespace KSPCompiler.Externals.Antlr.Cst
{
    // implementation of the unary operator node
    internal partial class CstConverterVisitor : KSPParserBaseVisitor<AstNode>
    {
        private static TNode SetupUnaryOperatorNode<TNode>( TNode dest )
            where TNode : AstExpressionSyntaxNode
        {
            dest.Left.Parent = dest;
            return dest;
        }

        private TNode VisitUnaryExpressionImpl<TNode>(
            ParserRuleContext context,
            ParserRuleContext left )
            where TNode : AstExpressionSyntaxNode, new()
        {
            var node = new TNode();

            node.Import( context );
            node.Left = left.Accept( this ) as AstExpressionSyntaxNode ?? AstExpressionSyntaxNode.Null;

            return SetupUnaryOperatorNode( node );
        }

        public override AstNode VisitUnaryExpression(KSPParser.UnaryExpressionContext context)
        {
            if( context.nested != null )
            {
                return context.nested.Accept( this );
            }

            AstExpressionSyntaxNode node = ( context.opr.Type ) switch
            {
                KSPLexer.MINUS =>
                    VisitUnaryExpressionImpl<AstUnaryMinusExpression>( context, context.unaryMinus ),
                KSPLexer.BIT_NOT =>
                    VisitUnaryExpressionImpl<AstUnaryNotExpression>( context, context.unaryNot ),
                _ =>
                    throw new System.ArgumentException( $"context.opr.Type is {context.opr.Text}" ),
            };
            return node;
        }
    }
}
