using System;
using System.Text;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.UseCases.Analysis.Evaluations.Declarations;

namespace KSPCompiler.Interactors.Tests.Analysis.Obfuscators;

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
