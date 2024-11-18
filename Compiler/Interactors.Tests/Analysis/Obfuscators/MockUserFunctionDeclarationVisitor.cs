using System;
using System.Text;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.UseCases.Analysis.Evaluations.Declarations;

namespace KSPCompiler.Interactors.Tests.Analysis.Obfuscators;

public class MockUserFunctionDeclarationVisitor : DefaultAstVisitor
{
    private StringBuilder Output { get; }
    private IUserFunctionDeclarationEvaluator Evaluator { get; set; } = new MockUserFunctionDeclarationEvaluator();

    public MockUserFunctionDeclarationVisitor( StringBuilder output )
    {
        Output = output;
    }

    public void Inject( IUserFunctionDeclarationEvaluator evaluator )
        => Evaluator = evaluator;

    public override IAstNode Visit( AstUserFunctionDeclarationNode node )
        => Evaluator.Evaluate( this, node );

    private class MockUserFunctionDeclarationEvaluator : IUserFunctionDeclarationEvaluator
    {
        public IAstNode Evaluate( IAstVisitor visitor, AstUserFunctionDeclarationNode node )
            => throw new NotImplementedException();
    }
}
