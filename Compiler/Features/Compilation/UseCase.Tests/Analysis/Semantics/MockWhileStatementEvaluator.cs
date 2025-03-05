using System;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Statements;
using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Semantics;

public class MockWhileStatementEvaluator : IWhileStatementEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstWhileStatementNode statement )
        => throw new NotImplementedException();
}
