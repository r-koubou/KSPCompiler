using System;

using KSPCompiler.Domain.Ast.Analyzers.Evaluators.KspUserFunctions;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

public class MockCallKspUserFunctionEvaluator : ICallKspUserFunctionEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstCallKspUserFunctionStatementNode statement )
    {
        throw new NotImplementedException();
    }
}
