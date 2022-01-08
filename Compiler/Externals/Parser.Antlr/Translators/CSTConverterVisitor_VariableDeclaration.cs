using KSPCompiler.Domain.Ast;
using KSPCompiler.Domain.Ast.Expressions;
using KSPCompiler.Domain.Ast.Statements;
using KSPCompiler.Externals.Parser.Antlr.Translators.Extensions;

#nullable disable

namespace KSPCompiler.Externals.Parser.Antlr.Translators
{
    // implementation of the variable declaration node
    public partial class CSTConverterVisitor
    {
        public override AstNode VisitVariableDeclaration( KSPParser.VariableDeclarationContext context )
        {
            var node = new AstVariableDeclaration();
            node.Import( context );
            node.Name        = context.name.Text;
            node.Modifier    = context.modifier?.Text;
            node.Initializer = context.variableInitializer()?.Accept( this ) as AstVariableInitializer;

            return node;
        }

        public override AstNode VisitVariableInitializer( KSPParser.VariableInitializerContext context )
        {
            var node = new AstVariableInitializer();
            var primitiveInitializer = context.primitiveInitializer();
            var arrayInitializer = context.arrayInitializer();

            node.Import( context );

            if( primitiveInitializer != null )
            {
                var initializer = primitiveInitializer.Accept( this ) as AstPrimitiveInitializer;
                node.PrimitiveInitializer = initializer;
                SetupChildNode( node, initializer, primitiveInitializer );
            }
            else
            {
                var initializer = arrayInitializer.Accept( this ) as AstArrayInitializer;
                SetupChildNode( node, initializer, arrayInitializer );
                node.ArrayInitializer = initializer;
            }

            return node;
        }

        public override AstNode VisitPrimitiveInitializer( KSPParser.PrimitiveInitializerContext context )
        {
            var expression = context.expression();
            var expressionList = context.expressionList();

            AstExpressionSyntaxNode expressionNode = null;
            AstExpressionList expressionListNode = null;

            // 初期化式：単一の式か複数の式かは型によってどちらか一方
            if( expression != null )
            {
                expressionNode = expression.Accept( this ) as AstExpressionSyntaxNode;
            }
            else if( expressionList != null )
            {
                expressionListNode = expressionList.Accept( this ) as AstExpressionList;
            }

            return new AstPrimitiveInitializer( null, expressionNode, expressionListNode );
        }

        public override AstNode VisitArrayInitializer( KSPParser.ArrayInitializerContext context )
        {
            AstArrayInitializer node = new AstArrayInitializer();

            var arraySizeExpression = context.expression();
            var expressionList = context.expressionList();

            node.Size = arraySizeExpression.Accept( this ) as AstExpressionSyntaxNode;

            if( expressionList != null )
            {
                node.Initializer = expressionList.Accept( this ) as AstExpressionList;
            }

            return node;
        }
    }
}
