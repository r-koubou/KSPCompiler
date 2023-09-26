using System.Diagnostics;

using KSPCompiler.Domain.Ast.Node;
using KSPCompiler.Domain.Ast.Node.Expressions;
using KSPCompiler.Domain.Ast.Node.Statements;
using KSPCompiler.Infrastructures.Parser.Antlr.Translators.Extensions;

namespace KSPCompiler.Infrastructures.Parser.Antlr.Translators
{
    // implementation of the variable declaration node
    public partial class CSTConverterVisitor
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

            AstExpressionSyntaxNode? expressionNode = null;
            AstExpressionList? expressionListNode = null;

            // 初期化式：単一の式か複数の式かは型によってどちらか一方
            if( expression != null )
            {
                expressionNode = expression.Accept( this ) as AstExpressionSyntaxNode;
                Debug.Assert( expressionNode != null );
            }
            else if( expressionList != null )
            {
                expressionListNode = expressionList.Accept( this ) as AstExpressionList;
                Debug.Assert( expressionList != null );
            }

            return new AstPrimitiveInitializer( null, expressionNode, expressionListNode );
        }

        public override AstNode VisitArrayInitializer( KSPParser.ArrayInitializerContext context )
        {
            var node = new AstArrayInitializer();

            var arraySizeExpression = context.expression();
            var expressionList = context.expressionList();

            node.Size = arraySizeExpression.Accept( this ) as AstExpressionSyntaxNode;
            Debug.Assert( node.Size != null );

            if( expressionList != null )
            {
                node.Initializer = expressionList.Accept( this ) as AstExpressionList;
                Debug.Assert( node.Initializer != null );
            }

            return node;
        }
    }
}
