using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.UseCases.Analysis.Evaluations.Symbols;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

internal class MockArrayElementEvaluator : IArrayElementEvaluator
{
    public MockArrayElementEvaluator() {}

    public IAstNode Evaluate( IAstVisitor visitor, AstArrayElementExpressionNode expr )
        => throw new NotImplementedException();
}
