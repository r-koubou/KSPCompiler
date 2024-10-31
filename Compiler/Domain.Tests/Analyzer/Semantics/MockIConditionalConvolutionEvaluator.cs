using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Conditions;
using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

public class MockIConditionalConvolutionEvaluator : IConditionalConvolutionEvaluator
{
    public bool? Evaluate( AstExpressionNode expr, bool workingValueForRecursive )
        => throw new System.NotImplementedException();
}
