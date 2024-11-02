using System;

using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Conditionals;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

public class MockContinueStatementEvaluator : IContinueStatementEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstContinueStatementNode statement )
        => throw new NotImplementedException();
}
