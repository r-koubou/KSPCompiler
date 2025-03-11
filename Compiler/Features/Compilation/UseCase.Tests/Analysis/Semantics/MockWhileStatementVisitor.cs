using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Statements;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Statements;

namespace KSPCompiler.Features.SymbolManagement.UseCase.Tests.Analysis.Semantics;

public class MockWhileStatementVisitor : DefaultAstVisitor
{
    private IWhileStatementEvaluator Evaluator { get; set; } = new MockWhileStatementEvaluator();

    public void Inject( IWhileStatementEvaluator evaluator )
    {
        Evaluator = evaluator;
    }

    public override IAstNode Visit( AstWhileStatementNode node )
        => Evaluator.Evaluate( this, node );
}
