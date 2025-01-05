using Antlr4.Runtime;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Infrastructures.Parser.Antlr.Translators.Extensions;

namespace KSPCompiler.Infrastructures.Parser.Antlr.Translators
{
    // implementation of the statement node
    public partial class CstConverterVisitor
    {
        private TNode VisitControlStatementImpl<TNode>(
            ParserRuleContext condition,
            ParserRuleContext block )
            where TNode : AstConditionalStatementNode, new()
        {
            var node = new TNode();
            var conditionNode = condition.Accept( this ) as AstExpressionNode;
            var blockNode = block.Accept( this ) as AstBlockNode;

            conditionNode ??= NullAstExpressionNode.Instance;
            blockNode     ??= NullAstBlockNode.Instance;

            node.Condition = conditionNode;
            node.CodeBlock = blockNode;

            SetupChildNode( node, conditionNode, condition );
            SetupChildNode( node, blockNode, block );

            return node;
        }

        public override AstNode VisitIfStatement( KSPParser.IfStatementContext context )
        {
            var node = VisitControlStatementImpl<AstIfStatementNode>( context.expression(), context.ifBlock );
            node.Import( tokenStream, context.ifBlock );

            if( context.elseBlock?.Accept( this ) is not AstBlockNode elseBlock )
            {
                return node;
            }

            node.ElseBlock = elseBlock;
            node.ElseBlock.Import( tokenStream, context.elseBlock! );

            return node;
        }

        public override AstNode VisitSelectStatement( KSPParser.SelectStatementContext context )
        {
            var caseList = context.caseBlock();
            var condition = context.expression();
            var node = new AstSelectStatementNode();

            #region Condition
            node.Import( tokenStream, context );
            node.Condition = condition.Accept( this ) as AstExpressionNode
                             ?? NullAstExpressionNode.Instance;

            node.Condition?.Import( tokenStream, condition );
            #endregion

            #region CaseBlock
            foreach( var c in caseList )
            {
                var caseBlock = c.Accept( this ) as AstCaseBlock
                                ?? NullAstCaseBlockNode.Instance;

                caseBlock.Parent = node;
                caseBlock.Import( tokenStream, c );
                node.CaseBlocks.Add( caseBlock );
            }
            #endregion

            return node;
        }

        public override AstNode VisitWhileStatement( KSPParser.WhileStatementContext context )
        {
            var node = VisitControlStatementImpl<AstWhileStatementNode>( context.expression(), context.block() );
            node.Import( tokenStream, context );

            return node;
        }

        public override AstNode VisitContinueStatement( KSPParser.ContinueStatementContext context )
        {
            var node = new AstContinueStatementNode();
            node.Import( tokenStream, context );

            return node;
        }

        public override AstNode VisitCallUserFunction( KSPParser.CallUserFunctionContext context )
        {
            var node = new AstCallUserFunctionStatementNode();
            node.Import( tokenStream, context );
            node.Name = context.name.Text;

            return node;
        }

        public override AstNode VisitExpressionStatement( KSPParser.ExpressionStatementContext context )
        {
            var assignment = context.assignmentExpression();
            var callExpression = context.callExpr;
            var callArguments = context.callArgs;

            //
            // Assignment
            //
            #region Assignment
            if( assignment != null )
            {
                return assignment.Accept( this );
            }
            #endregion ~Assignment
            //
            // Call command as statement
            //
            #region Call Command
            else if( callExpression != null )
            {
                return VisitCallCommand( context, callExpression, callArguments );
            }
            #endregion ~Call Command

            return NullAstExpressionNode.Instance;
        }
    }
}
