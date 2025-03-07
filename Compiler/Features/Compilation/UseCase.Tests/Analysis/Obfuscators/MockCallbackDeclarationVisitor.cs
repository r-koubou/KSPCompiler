using System;
using System.Text;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Declarations;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Blocks;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Obfuscators;

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
