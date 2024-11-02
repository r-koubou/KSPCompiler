using System;

using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Conditionals;
using KSPCompiler.Domain.Ast.Analyzers.Semantics;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

public class MockWhileStatementEvaluator : IWhileStatementEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstWhileStatementNode statement )
        => throw new NotImplementedException();
}
