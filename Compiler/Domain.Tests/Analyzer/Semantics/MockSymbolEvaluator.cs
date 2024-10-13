using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Symbols;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

internal class MockSymbolEvaluator : ISymbolEvaluator
{
    public IAstNode Evaluate( IAstVisitor<IAstNode> visitor, AstSymbolExpressionNode expr, AbortTraverseToken abortTraverseToken )
        => throw new System.NotImplementedException();
}
