using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.UseCases.Analysis.Evaluations.Conditionals;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

public class MockContinueStatementEvaluator : IContinueStatementEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstContinueStatementNode statement )
        => throw new NotImplementedException();
}
