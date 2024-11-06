using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.UseCases.Analysis.Evaluations.Conditionals;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

public class MockSelectStatementEvaluator : ISelectStatementEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstSelectStatementNode statement )
        => throw new NotImplementedException();
}
