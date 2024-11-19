using System;
using System.Text;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.UseCases.Analysis.Evaluations.Declarations;

namespace KSPCompiler.Interactors.Tests.Analysis.Obfuscators;

public class MockCallbackDeclarationVisitor : DefaultAstVisitor
{
    private StringBuilder Output { get; }
    private ICallbackDeclarationEvaluator Evaluator { get; set; } = new MockCallbackDeclarationEvaluator();

    public MockCallbackDeclarationVisitor( StringBuilder output )
    {
        Output = output;
    }

    public void Inject( ICallbackDeclarationEvaluator evaluator )
        => Evaluator = evaluator;

    public override IAstNode Visit( AstCallbackDeclarationNode node )
        => Evaluator.Evaluate( this, node );

    private class MockCallbackDeclarationEvaluator : ICallbackDeclarationEvaluator
    {
        public IAstNode Evaluate( IAstVisitor visitor, AstCallbackDeclarationNode node )
            => throw new NotImplementedException();
    }
}
