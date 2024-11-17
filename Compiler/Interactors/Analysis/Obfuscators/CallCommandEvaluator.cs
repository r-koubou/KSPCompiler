using System.Text;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Interactors.Analysis.Commons.Evaluations;
using KSPCompiler.Interactors.Analysis.Obfuscators.Extensions;
using KSPCompiler.UseCases.Analysis.Evaluations.Commands;

namespace KSPCompiler.Interactors.Analysis.Obfuscators;

public class CallCommandEvaluator : ICallCommandEvaluator
{
    private StringBuilder Output { get; }
    public CallCommandEvaluator( StringBuilder output )
    {
        Output = output;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstCallCommandExpressionNode node )
    {
        node.Left.Accept( visitor );
        Output.Append( '(' );

        if( node.Right is not AstExpressionListNode expressionList )
        {
            throw new AstAnalyzeException( node, $"Invalid command arguments node type. expect: {nameof(AstExpressionListNode)} actual: {node.Right.GetType().Name}" );
        }

        Output.AppendExpressionList( visitor, expressionList );

        Output.Append( ')' );

        return node;
    }
}
