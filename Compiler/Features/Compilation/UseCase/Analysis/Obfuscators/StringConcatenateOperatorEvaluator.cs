using System.Text;

using KSPCompiler.Features.Compilation.Domain.Ast.Nodes;
using KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Operators;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators;

public class StringConcatenateOperatorEvaluator : IStringConcatenateOperatorEvaluator
{
    private StringBuilder Output { get; }

    public StringConcatenateOperatorEvaluator( StringBuilder output )
    {
        Output = output;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstExpressionNode expr )
    {
        expr.Left.Accept( visitor );

        Output.Append( " & " );

        expr.Right.Accept( visitor );

        return expr;
    }
}
