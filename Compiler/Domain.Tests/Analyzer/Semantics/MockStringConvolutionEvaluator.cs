using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Strings;
using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

public class MockStringConvolutionEvaluator : IStringConvolutionEvaluator
{
    public string? Evaluate( AstExpressionNode expr, string workingValueForRecursive )
        => "abc";
}
