using System;

using Antlr4.Runtime;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Infrastructures.Parser.Antlr.Translators.Extensions;

namespace KSPCompiler.Infrastructures.Parser.Antlr.Translators
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

            node.Import( context );
            node.Left = (AstExpressionNode)left.Accept( this );

            return SetupUnaryOperatorNode( node );
        }

        public override AstNode VisitUnaryExpression(KSPParser.UnaryExpressionContext context)
        {
            if( context.nested != null )
            {
                return context.nested.Accept( this );
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
                    throw new ArgumentException( $"context.opr.Type is {context.opr.Text}" ),
            };
            return node;
        }
    }
}
