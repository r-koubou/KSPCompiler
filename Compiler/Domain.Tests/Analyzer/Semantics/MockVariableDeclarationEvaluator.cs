using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Declarations;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

public class MockVariableDeclarationEvaluator : IVariableDeclarationEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstVariableDeclarationNode node )
        => throw new System.NotImplementedException();
}
