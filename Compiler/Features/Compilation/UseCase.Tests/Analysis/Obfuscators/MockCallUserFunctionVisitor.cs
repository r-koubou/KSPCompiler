using System;
using System.Text;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.UserFunctions;
using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Obfuscators;

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
