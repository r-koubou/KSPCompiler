using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Symbols;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Semantics;

public class MockAstSymbolVisitor : DefaultAstVisitor
{
    private ISymbolEvaluator SymbolEvaluator { get; set; } = new MockSymbolEvaluator();
    private IArrayElementEvaluator ArrayElementEvaluator { get; set; } = new MockArrayElementEvaluator();

    public MockAstSymbolVisitor() {}

    public void Inject( ISymbolEvaluator evaluator )
    {
        SymbolEvaluator = evaluator;
    }

    public void Inject( IArrayElementEvaluator evaluator )
    {
        ArrayElementEvaluator = evaluator;
    }

    public override IAstNode Visit( AstSymbolExpressionNode node )
        => SymbolEvaluator.Evaluate( this, node );

    public override IAstNode Visit( AstArrayElementExpressionNode node )
        => ArrayElementEvaluator.Evaluate( this, node );
}
