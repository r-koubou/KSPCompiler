using System;

using KSPCompiler.Domain.Ast.Analyzers.Evaluators.UserFunctions;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

public class MockCallUserFunctionEvaluator : ICallUserFunctionEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstCallUserFunctionStatementNode statement )
    {
        throw new NotImplementedException();
    }
}
