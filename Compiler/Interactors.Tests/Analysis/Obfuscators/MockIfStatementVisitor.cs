using System;
using System.Text;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Interactors.Analysis.Obfuscators.Extensions;
using KSPCompiler.UseCases.Analysis.Evaluations.Statements;

namespace KSPCompiler.Interactors.Tests.Analysis.Obfuscators;

public class MockIfStatementVisitor : DefaultAstVisitor
{
    private StringBuilder Output { get; }

    private IIfStatementEvaluator Evaluator { get; set; } = new MockIfStatementEvaluator();

    public MockIfStatementVisitor( StringBuilder output )
    {
        Output = output;
    }

    public void Inject( IIfStatementEvaluator evaluator )
    {
        Evaluator = evaluator;
    }

    public override IAstNode Visit( AstIfStatementNode node )
        => Evaluator.Evaluate( this, node );

    public override IAstNode Visit( AstEqualExpressionNode node )
    {
        node.Left.Accept( this );
        Output.Append( '=' );
        node.Right.Accept( this );
        return node;
    }

    public override IAstNode Visit( AstIntLiteralNode node )
    {
        Output.Append( node.Value );
        return node;
    }

    public override IAstNode Visit( AstSymbolExpressionNode node )
    {
        Output.Append( node.Name );
        return node;
    }

    public override IAstNode Visit( AstCallCommandExpressionNode node )
    {
        node.Left.Accept( this );
        Output.Append( "()" );
        Output.NewLine();

        return node;
    }

    private class MockIfStatementEvaluator : IIfStatementEvaluator
    {
        public IAstNode Evaluate( IAstVisitor visitor, AstIfStatementNode statement )
            => throw new NotImplementedException();
    }
}
