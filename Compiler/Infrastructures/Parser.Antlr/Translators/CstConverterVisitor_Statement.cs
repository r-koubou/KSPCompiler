using System.Diagnostics;

using Antlr4.Runtime;

using KSPCompiler.Domain.Ast.Node;
using KSPCompiler.Domain.Ast.Node.Blocks;
using KSPCompiler.Domain.Ast.Node.Statements;
using KSPCompiler.Infrastructures.Parser.Antlr.Translators.Extensions;

namespace KSPCompiler.Infrastructures.Parser.Antlr.Translators
{
    // implementation of the statement node
    public partial class CstConverterVisitor
    {
        private TNode VisitControlStatementImpl<TNode>(
            ParserRuleContext condition,
            ParserRuleContext block )
            where TNode : AstConditionalStatement, new()
        {
            var node = new TNode();
            var conditionNode = condition.Accept( this ) as AstExpressionSyntaxNode;
            var blockNode = block.Accept( this ) as AstBlock;

            Debug.Assert( conditionNode != null );
            Debug.Assert( blockNode != null );

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
            node.ElseBlock.Import( context.elseBlock! );

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
            Debug.Assert( node.Condition != null );

            node.Condition?.Import( condition );
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

        public override AstNode VisitContinueStatement( KSPParser.ContinueStatementContext context )
        {
            var node = new AstContinueStatement();
            node.Import( context );

            return node;
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
