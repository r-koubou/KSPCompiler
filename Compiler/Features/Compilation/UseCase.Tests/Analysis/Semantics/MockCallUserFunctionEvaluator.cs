using System;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.UserFunctions;
using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Semantics;

public class MockCallUserFunctionEvaluator : ICallUserFunctionEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstCallUserFunctionStatementNode statement )
    {
        throw new NotImplementedException();
    }
}
