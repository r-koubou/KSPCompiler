using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.UseCases.Analysis.Evaluations.Conditionals;

namespace KSPCompiler.Interactor.Tests.Analysis.Semantics;

public class MockWhileStatementEvaluator : IWhileStatementEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstWhileStatementNode statement )
        => throw new NotImplementedException();
}
