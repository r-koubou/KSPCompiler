using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.UseCases.Analysis.Evaluations.Conditionals;

namespace KSPCompiler.Interactor.Tests.Analysis.Semantics;

public class MockIfStatementEvaluator : IIfStatementEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstIfStatementNode statement )
        => throw new NotImplementedException();
}
