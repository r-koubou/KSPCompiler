using System;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Declarations;
using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Semantics;

public class MockVariableDeclarationEvaluator : IVariableDeclarationEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstVariableDeclarationNode node )
        => throw new NotImplementedException();
}
