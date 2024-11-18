using System;
using System.Text;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.UseCases.Analysis.Evaluations.UserFunctions;

namespace KSPCompiler.Interactors.Tests.Analysis.Obfuscators;

public class MockCallUserFunctionVisitor : DefaultAstVisitor
{
    private StringBuilder Output { get; }
    private ICallUserFunctionEvaluator Evaluator { get; set; } = new MockCallUserFunctionEvaluator();

    public MockCallUserFunctionVisitor( StringBuilder output )
    {
        Output    = output;
    }

    public void Inject( ICallUserFunctionEvaluator evaluator )
        => Evaluator = evaluator;

    public override IAstNode Visit( AstCallUserFunctionStatementNode node )
        => Evaluator.Evaluate( this, node );

    private class MockCallUserFunctionEvaluator : ICallUserFunctionEvaluator
    {
        public IAstNode Evaluate( IAstVisitor visitor, AstCallUserFunctionStatementNode statement )
            => throw new NotImplementedException();
    }
}
