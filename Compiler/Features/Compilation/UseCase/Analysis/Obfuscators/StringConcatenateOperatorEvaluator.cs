using System.Text;

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
