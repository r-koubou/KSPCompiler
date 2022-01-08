#nullable enable

using System.Diagnostics;

using Antlr4.Runtime;

using KSPCompiler.Domain.Ast;
using KSPCompiler.Domain.Ast.Blocks;
using KSPCompiler.Domain.Ast.Statements;
using KSPCompiler.Externals.Parser.Antlr.Translators.Extensions;

namespace KSPCompiler.Externals.Parser.Antlr.Translators
{
    // implementation of the statement node
    public partial class CSTConverterVisitor
    {
        private TNode VisitControlStatementImpl<TNode>(
            ParserRuleContext condition,
            ParserRuleContext block )
            where TNode : AstConditionalStatement, new()
        {
            var node = new TNode();
            var conditionNode = condition.Accept( this ) as AstExpressionSyntaxNode;
            var blockNode = block.Accept( this ) as AstBlock;

            node.Condition = conditionNode;
            node.CodeBlock = blockNode;

            SetupChildNode( node, conditionNode, condition );
            SetupChildNode( node, blockNode, block );

            return node;
        }

        public override AstNode VisitIfStatement( KSPParser.IfStatementContext context )
        {
            var node = VisitControlStatementImpl<AstIfStatement>( context.expression(), context.ifBlock );
            var elseBlock = context.elseBlock?.Accept( this ) as AstBlock;

            if( elseBlock == null )
            {
                return node;
            }

            node.ElseBlock = elseBlock;
            node.ElseBlock.Import( context.elseBlock );

            return node;
        }

        public override AstNode VisitSelectStatement( KSPParser.SelectStatementContext context )
        {
            var caseList = context.caseBlock();
            var condition = context.expression();
            var node = new AstSelectStatement();

            #region Condition
            node.Import( context );
            node.Condition = condition.Accept( this ) as AstExpressionSyntaxNode;
            node.Condition.Import( condition );
            #endregion

            #region CaseBlock
            foreach( var c in caseList )
            {
                var caseBlock = c.Accept( this ) as AstCaseBlock;

                Debug.Assert( caseBlock != null );

                caseBlock.Parent = node;
                caseBlock.Import( c );
                node.CaseBlocks.Add( caseBlock );
            }
            #endregion

            return node;
        }

        public override AstNode VisitWhileStatement( KSPParser.WhileStatementContext context )
        {
            return VisitControlStatementImpl<AstWhileStatement>( context.expression(), context.block() );
        }

        public override AstNode VisitCallKspUserFunction( KSPParser.CallKspUserFunctionContext context )
        {
            var node = new AstCallKspUserFunctionStatement();
            node.Import( context );
            node.Name = context.name.Text;

            return node;
        }


    }
}
