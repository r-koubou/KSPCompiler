using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Infrastructures.Parser.Antlr.Translators.Extensions;

namespace KSPCompiler.Infrastructures.Parser.Antlr.Translators
{
    // implementation of the variable declaration node
    public partial class CstConverterVisitor
    {
        public override AstNode VisitVariableDeclaration( KSPParser.VariableDeclarationContext context )
        {
            var node = new AstVariableDeclaration();
            node.Import( context );
            node.Name     = context.name.Text;
            node.Modifier = string.Empty;

            if( context.modifier?.Text != null )
            {
                node.Modifier = context.modifier.Text;
            }

            var initializer = context.variableInitializer()?.Accept( this );

            if( initializer != null )
            {
                node.Initializer = (AstVariableInitializer)initializer;
            }

            return node;
        }

        public override AstNode VisitVariableInitializer( KSPParser.VariableInitializerContext context )
        {
            var node = new AstVariableInitializer();
            var primitiveInitializer = context.primitiveInitializer();
            var arrayInitializer = context.arrayInitializer();

            node.Import( context );

            if( primitiveInitializer?.Accept( this ) is AstPrimitiveInitializer astPrimitiveInitializer )
            {
                node.PrimitiveInitializer = astPrimitiveInitializer;
                SetupChildNode( node, astPrimitiveInitializer, primitiveInitializer );
            }

            if( arrayInitializer?.Accept( this ) is AstArrayInitializer astArrayInitializer )
            {
                SetupChildNode( node, astArrayInitializer, arrayInitializer );
                node.ArrayInitializer = astArrayInitializer;
            }

            return node;
        }

        public override AstNode VisitPrimitiveInitializer( KSPParser.PrimitiveInitializerContext context )
        {
            var expression = context.expression();
            var expressionList = context.expressionList();

            AstExpressionSyntaxNode expressionNode = NullAstExpressionSyntaxNode.Instance;
            AstExpressionList expressionListNode = new AstExpressionList();

            // 初期化式：単一の式か複数の式かは型によってどちらか一方
            if( expression != null )
            {
                expressionNode = expression.Accept( this ) as AstExpressionSyntaxNode
                                 ?? throw new MustBeNotNullException( nameof( expressionNode ) );
            }
            else if( expressionList != null )
            {
                expressionListNode = expressionList.Accept( this ) as AstExpressionList
                                     ?? throw new MustBeNotNullException( nameof( expressionListNode ) );
            }

            return new AstPrimitiveInitializer( NullAstNode.Instance, expressionNode, expressionListNode );
        }

        public override AstNode VisitArrayInitializer( KSPParser.ArrayInitializerContext context )
        {
            var node = new AstArrayInitializer();

            var arraySizeExpression = context.expression();
            var expressionList = context.expressionList();

            node.Size = arraySizeExpression.Accept( this ) as AstExpressionSyntaxNode
                        ?? throw new MustBeNotNullException( nameof( node.Size ) );

            if( expressionList != null )
            {
                node.Initializer = expressionList.Accept( this ) as AstExpressionList
                                   ?? throw new MustBeNotNullException( nameof( node.Initializer ) );
            }

            return node;
        }
    }
}
