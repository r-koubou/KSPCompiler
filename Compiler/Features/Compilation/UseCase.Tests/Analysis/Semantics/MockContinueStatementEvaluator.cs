using System;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Statements;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Statements;

namespace KSPCompiler.Features.SymbolManagement.UseCase.Tests.Analysis.Semantics;

public class MockContinueStatementEvaluator : IContinueStatementEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstContinueStatementNode statement )
        => throw new NotImplementedException();
}
