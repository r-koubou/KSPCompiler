using System.Collections.Generic;
using System.Linq;

using KSPCompiler.Commons.Text;
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
            var comments = GetCommentsToLeft( context );
            var commentLines = new List<string>();

            if( comments.Any() )
            {
                commentLines.AddRange( GetCommentText( comments.Last().Text ) );
            }

            var node = new AstVariableDeclarationNode();
            node.Import( tokenStream, context );
            node.Name = context.name.Text;
            node.CommentLines = commentLines;

            node.VariableNamePosition = new Position
            {
                BeginLine   = context.name.Line,
                BeginColumn = context.name.Column,
                EndLine     = context.name.Line,
                EndColumn   = context.name.Column + context.name.Text.Length
            };

            if( context.modifier?.Accept( this ) is AstModiferNode modifier )
            {
                node.Modifier = modifier;
            }

            var initializer = context.variableInitializer()?.Accept( this );

            if( initializer != null )
            {
                node.Initializer = (AstVariableInitializerNode)initializer;
            }

            return node;
        }

        public override AstNode VisitDeclarationModifier( KSPParser.DeclarationModifierContext context )
        {
            var modifiers = new List<string>();

            foreach( var x in context.children )
            {
                modifiers.Add( x.GetText() );
            }

            var node = new AstModiferNode( modifiers );
            node.Import( tokenStream, context );

            return node;
        }

        public override AstNode VisitVariableInitializer( KSPParser.VariableInitializerContext context )
        {
            var node = new AstVariableInitializerNode();
            var primitiveInitializer = context.primitiveInitializer();
            var arrayInitializer = context.arrayInitializer();

            node.Import( tokenStream, context );

            if( primitiveInitializer?.Accept( this ) is AstPrimitiveInitializerNode astPrimitiveInitializer )
            {
                SetupChildNode( node, astPrimitiveInitializer, primitiveInitializer );
                node.PrimitiveInitializer = astPrimitiveInitializer;
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
            var uiInitializer = context.uiInitializer();

            AstExpressionNode expressionNode = NullAstExpressionNode.Instance;
            AstExpressionListNode uiInitializerNode = new AstExpressionListNode();

            // 初期化式：単一の式かUI初期化式かどちらか一方
            if( expression != null )
            {
                expressionNode = expression.Accept( this ) as AstExpressionNode
                                 ?? NullAstExpressionNode.Instance;
            }
            else if( uiInitializer != null )
            {
                uiInitializerNode = uiInitializer.Accept( this ) as AstExpressionListNode
                                     ?? NullAstExpressionListNode.Instance;
            }

            return new AstPrimitiveInitializerNode( NullAstNode.Instance, expressionNode, uiInitializerNode );
        }

        public override AstNode VisitArrayInitializer( KSPParser.ArrayInitializerContext context )
        {
            var node = new AstArrayInitializerNode();

            var arraySizeExpression = context.expression();
            var expressionList = context.expressionList();

            node.Size = arraySizeExpression.Accept( this ) as AstExpressionNode
                        ?? NullAstExpressionNode.Instance;

            node.HasAssignOperator = context.ASSIGN() != null;

            if( expressionList != null )
            {
                node.Initializer = expressionList.Accept( this ) as AstExpressionListNode
                                   ?? NullAstExpressionListNode.Instance;
            }

            node.Import( tokenStream, context );

            return node;
        }

        public override AstNode VisitUiInitializer( KSPParser.UiInitializerContext context )
        {
            var node = new AstExpressionListNode();

            var expressionList = context.expressionList();

            if( expressionList != null )
            {
                var expressionListNode = expressionList.Accept( this ) as AstExpressionListNode
                    ?? NullAstExpressionListNode.Instance;

                node.Expressions.AddRange( expressionListNode.Expressions );
            }

            return node;
        }
    }
}
