using System;

using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Operators;
using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

public class MockStringConcatenateOperatorEvaluator : IStringConcatenateOperatorEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstExpressionNode expr )
        => throw new NotImplementedException();
}
