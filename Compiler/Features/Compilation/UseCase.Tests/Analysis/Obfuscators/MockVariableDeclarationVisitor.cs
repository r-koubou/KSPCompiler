using System;
using System.Text;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Declarations;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Statements;

namespace KSPCompiler.Features.SymbolManagement.UseCase.Tests.Analysis.Obfuscators;

public class MockVariableDeclarationVisitor : DefaultAstVisitor
{
    private StringBuilder OutputBuilder { get; }
    private IVariableDeclarationEvaluator Evaluator { get; set; } = new MockVariableDeclarationEvaluator();

    public MockVariableDeclarationVisitor( StringBuilder outputBuilder )
    {
        OutputBuilder = outputBuilder;
    }

    public void Inject( IVariableDeclarationEvaluator evaluator )
    {
        Evaluator = evaluator;
    }

    public override IAstNode Visit( AstVariableDeclarationNode node )
        => Evaluator.Evaluate( this, node );

    public override IAstNode Visit( AstIntLiteralNode node )
    {
        OutputBuilder.Append( node.Value );
        return node;
    }

    private class MockVariableDeclarationEvaluator : IVariableDeclarationEvaluator
    {
        public IAstNode Evaluate( IAstVisitor visitor, AstVariableDeclarationNode node )
            => throw new NotImplementedException();
    }
}
