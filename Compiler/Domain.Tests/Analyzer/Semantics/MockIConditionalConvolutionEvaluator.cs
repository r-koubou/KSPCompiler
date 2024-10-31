using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Conditions;
using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

public class MockIConditionalConvolutionEvaluator : IConditionalConvolutionEvaluator
{
    private IConditionalConvolutionCalculator Calculator { get; set; } = new MockConditionalConvolutionCalculator();

    public void Inject( IConditionalConvolutionCalculator calculator )
        => Calculator = calculator;

    public bool? Evaluate( AstExpressionNode expr, bool workingValueForRecursive )
        => Calculator.Calculate( expr );
}
