using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.UseCases.Analysis.Evaluations.Declarations;

namespace KSPCompiler.Interactor.Tests.Analysis.Semantics;

public class MockCallbackDeclarationEvaluator : ICallbackDeclarationEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstCallbackDeclarationNode node )
        => throw new NotImplementedException();
}
