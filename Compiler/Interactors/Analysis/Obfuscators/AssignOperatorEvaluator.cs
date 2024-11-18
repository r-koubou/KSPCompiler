using System.Text;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Interactors.Analysis.Obfuscators.Extensions;
using KSPCompiler.UseCases.Analysis.Evaluations.Operators;

namespace KSPCompiler.Interactors.Analysis.Obfuscators;

public class AssignOperatorEvaluator :  IAssignOperatorEvaluator
{
    private StringBuilder Output { get; }

    public AssignOperatorEvaluator( StringBuilder output )
    {
        Output = output;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstExpressionNode expr )
    {
        expr.Left.Accept( visitor );

        Output.Append( " := " );

        expr.Right.Accept( visitor );

        Output.NewLine();

        return expr;
    }
}
