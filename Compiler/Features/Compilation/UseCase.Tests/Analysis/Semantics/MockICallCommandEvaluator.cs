using System;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Commands;
using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Semantics;

internal class MockICallCommandEvaluator : ICallCommandEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstCallCommandExpressionNode node )
        => throw new NotImplementedException();
}
