using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.UseCases.Analysis.Evaluations.UserFunctions;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

public class MockCallUserFunctionEvaluator : ICallUserFunctionEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstCallUserFunctionStatementNode statement )
    {
        throw new NotImplementedException();
    }
}
