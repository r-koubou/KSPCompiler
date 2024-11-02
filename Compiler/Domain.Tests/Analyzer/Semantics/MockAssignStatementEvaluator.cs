using System;

using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Assigns;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

public class MockAssignStatementEvaluator : IAssignStatementEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstAssignStatementNode statement )
    {
        throw new NotImplementedException();
    }
}
