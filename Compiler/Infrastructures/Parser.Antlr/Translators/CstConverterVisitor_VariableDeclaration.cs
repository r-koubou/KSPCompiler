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
            var node = new AstVariableDeclarationNode();
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
                node.Initializer = (AstVariableInitializerNode)initializer;
            }

            return node;
        }

        public override AstNode VisitVariableInitializer( KSPParser.VariableInitializerContext context )
        {
            var node = new AstVariableInitializerNode();
            var primitiveInitializer = context.primitiveInitializer();
            var arrayInitializer = context.arrayInitializer();

            node.Import( context );

            if( primitiveInitializer?.Accept( this ) is AstPrimitiveInitializerNode astPrimitiveInitializer )
            {
                node.PrimitiveInitializer = astPrimitiveInitializer;
                SetupChildNode( node, astPrimitiveInitializer, primitiveInitializer );
            }

            if( arrayInitializer?.Accept( this ) is AstArrayInitializerNode astArrayInitializer )
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

            AstExpressionNode expressionNode = NullAstExpressionNode.Instance;
            AstExpressionListNode expressionListNode = new AstExpressionListNode();

            // 初期化式：単一の式か複数の式かは型によってどちらか一方
            if( expression != null )
            {
                expressionNode = expression.Accept( this ) as AstExpressionNode
                                 ?? throw new MustBeNotNullException( nameof( expressionNode ) );
            }
            else if( expressionList != null )
            {
                expressionListNode = expressionList.Accept( this ) as AstExpressionListNode
                                     ?? throw new MustBeNotNullException( nameof( expressionListNode ) );
            }

            return new AstPrimitiveInitializerNode( NullAstNode.Instance, expressionNode, expressionListNode );
        }

        public override AstNode VisitArrayInitializer( KSPParser.ArrayInitializerContext context )
        {
            var node = new AstArrayInitializerNode();

            var arraySizeExpression = context.expression();
            var expressionList = context.expressionList();

            node.Size = arraySizeExpression.Accept( this ) as AstExpressionNode
                        ?? throw new MustBeNotNullException( nameof( node.Size ) );

            if( expressionList != null )
            {
                node.Initializer = expressionList.Accept( this ) as AstExpressionListNode
                                   ?? throw new MustBeNotNullException( nameof( node.Initializer ) );
            }

            return node;
        }
    }
}
