using System;

using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Conditionals;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

public class MockIfStatementEvaluator : IIfStatementEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstIfStatementNode statement )
        => throw new NotImplementedException();
}
