using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.UseCases.Analysis.Evaluations.Declarations;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

public class MockUserFunctionDeclarationEvaluator : IUserFunctionDeclarationEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstUserFunctionDeclarationNode node )
        => throw new NotImplementedException();
}
