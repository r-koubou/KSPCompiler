using System;

using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Integers;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Reals;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Strings;
using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

public class MockIntegerConvolutionEvaluator : IIntegerConvolutionEvaluator
{
    public int? Evaluate( AstExpressionNode expr, int workingValueForRecursive )
        => throw new NotImplementedException();
}

public class MockRealConvolutionEvaluator : IRealConvolutionEvaluator
{
    public double? Evaluate( AstExpressionNode expr, double workingValueForRecursive )
        => throw new NotImplementedException();
}

public class MockStringConvolutionEvaluator : IStringConvolutionEvaluator
{
    public string? Evaluate( AstExpressionNode expr, string workingValueForRecursive )
        => throw new NotImplementedException();
}
