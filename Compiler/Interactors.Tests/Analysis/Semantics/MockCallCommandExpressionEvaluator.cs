using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.UseCases.Analysis.Evaluations.Commands;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

internal class MockCallCommandExpressionEvaluator : ICallCommandExpressionEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstCallCommandExpressionNode node )
        => throw new NotImplementedException();
}
