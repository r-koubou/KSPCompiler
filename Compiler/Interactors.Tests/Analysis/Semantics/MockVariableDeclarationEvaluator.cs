using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.UseCases.Analysis.Evaluations.Declarations;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

public class MockVariableDeclarationEvaluator : IVariableDeclarationEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstVariableDeclarationNode node )
        => throw new NotImplementedException();
}
