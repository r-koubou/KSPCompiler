using System.Collections.Generic;

using Antlr4.Runtime.Tree;

using KSPCompiler.Commons.Text;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Infrastructures.Parser.Antlr.Translators.Extensions;

namespace KSPCompiler.Infrastructures.Parser.Antlr.Translators
{
    // Implementation of root, callback and user-defined function node generation
    public partial class CstConverterVisitor
    {
        public override AstNode VisitCompilationUnit( KSPParser.CompilationUnitContext context )
        {
            var node = new AstCompilationUnitNode();
            var list = new List<IParseTree>();

            node.Import( tokenStream, context );
            list.AddRange( context.callbackDeclaration() );
            list.AddRange( context.userFunctionDeclaration() );

            return SetupChildrenNode( node, node.GlobalBlocks, list );
        }

        public override AstNode VisitCallbackDeclaration( KSPParser.CallbackDeclarationContext context )
        {
            var node = new AstCallbackDeclarationNode();

            node.Import( tokenStream, context );
            node.Name     = context.name.Text;
            node.Position = ToPosition( context );
            node.Block    = context.block().Accept( this ) as AstBlockNode
                            ?? throw new MustBeNotNullException( nameof( node.Block ) );

            node.Block.Parent = node;

            if( context.arguments != null )
            {
                node.ArgumentList = context.arguments.Accept( this ) as AstArgumentListNode
                                    ?? throw new MustBeNotNullException( nameof( node.ArgumentList ) );

                node.ArgumentList.Parent = node;
            }

            return node;
        }

        public override AstNode VisitUserFunctionDeclaration( KSPParser.UserFunctionDeclarationContext context )
        {
            var node = new AstUserFunctionDeclarationNode();

            node.Import( tokenStream, context );
            node.Name                 = context.name.Text;

            node.Position             = ToPosition( context );
            node.FunctionNamePosition = new Position
            {
                BeginLine   = context.name.Line,
                BeginColumn = context.name.Column,
                EndLine     = context.name.Line,
                EndColumn   = context.name.Column + context.name.Text.Length
            };

            node.Block        = context.block().Accept( this ) as AstBlockNode
                                ?? throw new MustBeNotNullException( nameof( node.Block ) );

            node.Block.Parent = node;

            return node;
        }

        public override AstNode VisitArgumentDefinitionList( KSPParser.ArgumentDefinitionListContext context )
        {
            var node = new AstArgumentListNode();

            node.Import( tokenStream, context );
            VisitArgumentDefinitionListRecursive( context, node );

            return node;
        }

        private void VisitArgumentDefinitionListRecursive(
            KSPParser.ArgumentDefinitionListContext context,
            AstArgumentListNode destNode )
        {
            var argumentList = context.argumentDefinitionList();
            var identifier = context.IDENTIFIER();

            var arg = new AstArgumentNode();

            arg.Import( tokenStream, identifier );
            arg.Name = identifier.GetText();

            //
            // argumentDefinitionList COMMA IDENTIFIER
            //
            if( argumentList != null )
            {
                VisitArgumentDefinitionListRecursive(
                    argumentList,
                    destNode
                );

                destNode.Arguments.Add( arg );
            }
            // IDENTIFIER
            else
            {
                destNode.Arguments.Add( arg );
            }
        }

        public override AstNode VisitBlock( KSPParser.BlockContext context )
        {
            var node = new AstBlockNode();

            node.Import( tokenStream, context );

            return SetupChildrenNode(
                node,
                node.Statements,
                new List<IParseTree>( context.statement() )
            );
        }

        public override AstNode VisitCaseBlock( KSPParser.CaseBlockContext context )
        {
            var node = new AstCaseBlock();
            var condFrom = context.condFrom;
            var condTo = context.condTo;
            var codeBlock = context.block();

            node.Import( tokenStream, context );

            if( condFrom?.Accept( this ) is AstExpressionNode condFromNode )
            {
                node.ConditionFrom = condFromNode;
                SetupChildNode( node, node.ConditionFrom, condFrom );
            }

            if( condTo?.Accept( this ) is AstExpressionNode conditionToNode )
            {
                node.ConditionTo = conditionToNode;
                SetupChildNode( node, node.ConditionTo, condTo );
            }

            if( codeBlock?.Accept( this ) is AstBlockNode codeBlockNode )
            {
                node.CodeBlock = codeBlockNode;
                SetupChildNode( node, node.CodeBlock, codeBlock );
            }

            return node;
        }
    }
}
