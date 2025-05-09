using System;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Declarations;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Blocks;

namespace KSPCompiler.Features.SymbolManagement.UseCase.Tests.Analysis.Semantics;

public class MockCallbackDeclarationEvaluator : ICallbackDeclarationEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstCallbackDeclarationNode node )
        => throw new NotImplementedException();
}
