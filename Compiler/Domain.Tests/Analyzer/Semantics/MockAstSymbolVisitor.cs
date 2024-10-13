using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Symbols;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

public class MockAstSymbolVisitor : DefaultAstVisitor
{
    private ISymbolEvaluator SymbolEvaluator { get; set; } = new MockSymbolEvaluator();

    public MockAstSymbolVisitor() {}

    public void Inject( ISymbolEvaluator unaryOperatorEvaluator )
    {
        SymbolEvaluator = unaryOperatorEvaluator;
    }

    public override IAstNode Visit( AstSymbolExpressionNode node, AbortTraverseToken abortTraverseToken )
        => SymbolEvaluator.Evaluate( this, node, abortTraverseToken );
}
