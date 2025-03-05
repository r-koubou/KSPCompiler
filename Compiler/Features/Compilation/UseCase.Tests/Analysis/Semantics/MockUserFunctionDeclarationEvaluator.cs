using System;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Declarations;
using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Ast.Nodes.Blocks;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Semantics;

public class MockUserFunctionDeclarationEvaluator : IUserFunctionDeclarationEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstUserFunctionDeclarationNode node )
        => throw new NotImplementedException();
}
