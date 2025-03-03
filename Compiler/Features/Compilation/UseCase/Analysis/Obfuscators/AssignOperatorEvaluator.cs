using System.Text;

using KSPCompiler.Features.Compilation.Domain.Ast.Nodes;
using KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Operators;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators.Extensions;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators;

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
