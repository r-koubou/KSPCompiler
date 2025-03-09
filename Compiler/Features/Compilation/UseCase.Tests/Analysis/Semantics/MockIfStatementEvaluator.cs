using System;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Statements;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Statements;

namespace KSPCompiler.Features.SymbolManagement.UseCase.Tests.Analysis.Semantics;

public class MockIfStatementEvaluator : IIfStatementEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstIfStatementNode statement )
        => throw new NotImplementedException();
}
