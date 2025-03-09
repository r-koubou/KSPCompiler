using System;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Symbols;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;

namespace KSPCompiler.Features.SymbolManagement.UseCase.Tests.Analysis.Semantics;

internal class MockArrayElementEvaluator : IArrayElementEvaluator
{
    public MockArrayElementEvaluator() {}

    public IAstNode Evaluate( IAstVisitor visitor, AstArrayElementExpressionNode expr )
        => throw new NotImplementedException();
}
