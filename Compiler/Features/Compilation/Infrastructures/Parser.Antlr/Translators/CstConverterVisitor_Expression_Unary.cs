using Antlr4.Runtime;

using KSPCompiler.Features.Compilation.Infrastructures.Parser.Antlr.Translators.Extensions;
using KSPCompiler.Infrastructures.Parser.Antlr;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;

namespace KSPCompiler.Features.Compilation.Infrastructures.Parser.Antlr.Translators
{
    // implementation of the unary operator node
    public partial class CstConverterVisitor
    {
        private TNode SetupUnaryOperatorNode<TNode>( TNode dest )
            where TNode : AstExpressionNode
        {
            if( dest.Left != null )
            {
                dest.Left.Parent = dest;
            }

            return dest;
        }

        private TNode VisitUnaryExpressionImpl<TNode>(
            ParserRuleContext context,
            ParserRuleContext left )
            where TNode : AstExpressionNode, new()
        {
            var node = new TNode();

            node.Import( tokenStream, context );
            node.Left = left.Accept( this ) as AstExpressionNode
                        ?? NullAstExpressionNode.Instance;

            return SetupUnaryOperatorNode( node );
        }

        public override AstNode VisitUnaryExpression(KSPParser.UnaryExpressionContext context)
        {
            if( context.nested != null )
            {
                return context.nested.Accept( this );
            }

            if( context.opr == null )
            {
                return NullAstExpressionNode.Instance;
            }

            AstExpressionNode node = ( context.opr.Type ) switch
            {
                KSPLexer.MINUS =>
                    VisitUnaryExpressionImpl<AstUnaryMinusExpressionNode>( context, context.unaryMinus ),
                KSPLexer.BIT_NOT =>
                    VisitUnaryExpressionImpl<AstUnaryNotExpressionNode>( context, context.unaryNot ),
                KSPLexer.BOOL_NOT =>
                    VisitUnaryExpressionImpl<AstUnaryLogicalNotExpressionNode>( context, context.logicalNot ),
                _ =>
                    NullAstExpressionNode.Instance
            };
            return node;
        }
    }
}
