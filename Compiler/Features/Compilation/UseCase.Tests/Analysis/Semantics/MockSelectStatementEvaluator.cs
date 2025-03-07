using System;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Statements;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Statements;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Semantics;

public class MockSelectStatementEvaluator : ISelectStatementEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstSelectStatementNode statement )
        => throw new NotImplementedException();
}
