#nullable disable

using System;

using Antlr4.Runtime;

using KSPCompiler.Domain.Ast;
using KSPCompiler.Domain.Ast.Expressions;
using KSPCompiler.Externals.Parser.Antlr.Translators.Extensions;

namespace KSPCompiler.Externals.Parser.Antlr.Translators
{
    // implementation of the unary operator node
    public partial class CSTConverterVisitor
    {
        private TNode SetupUnaryOperatorNode<TNode>( TNode dest )
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
            node.Left = left.Accept( this ) as AstExpressionSyntaxNode;

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
                    throw new ArgumentException( $"context.opr.Type is {context.opr.Text}" ),
            };
            return node;
        }
    }
}
