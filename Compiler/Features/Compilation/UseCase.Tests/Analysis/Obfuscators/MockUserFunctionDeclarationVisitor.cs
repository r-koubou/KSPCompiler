using System;
using System.Text;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Declarations;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Blocks;

namespace KSPCompiler.Features.SymbolManagement.UseCase.Tests.Analysis.Obfuscators;

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
