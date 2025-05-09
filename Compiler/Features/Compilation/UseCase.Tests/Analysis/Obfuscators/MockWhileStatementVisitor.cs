using System;
using System.Text;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Statements;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Statements;

namespace KSPCompiler.Features.SymbolManagement.UseCase.Tests.Analysis.Obfuscators;

public class MockWhileStatementVisitor : DefaultAstVisitor
{
    private StringBuilder Output { get; }
    private IWhileStatementEvaluator WhileEvaluator { get; set; } = new MockWhileStatementEvaluator();
    private IContinueStatementEvaluator ContinueEvaluator { get; set; } = new MockContinueStatementEvaluator();

    public MockWhileStatementVisitor( StringBuilder output )
    {
        Output = output;
    }

    public void Inject( IWhileStatementEvaluator evaluator )
    {
        WhileEvaluator = evaluator;
    }

    public void Inject( IContinueStatementEvaluator evaluator )
    {
        ContinueEvaluator = evaluator;
    }

    public override IAstNode Visit( AstWhileStatementNode node )
        => WhileEvaluator.Evaluate( this, node );

    public override IAstNode Visit( AstContinueStatementNode node )
        => ContinueEvaluator.Evaluate( this, node );

    public override IAstNode Visit( AstNotEqualExpressionNode node )
    {
        node.Left.Accept( this );
        Output.Append( '#' );
        node.Right.Accept( this );

        return node;
    }

    public override IAstNode Visit( AstIntLiteralNode node )
    {
        Output.Append( node.Value );
        return node;
    }

    private class MockWhileStatementEvaluator : IWhileStatementEvaluator
    {
        public IAstNode Evaluate( IAstVisitor visitor, AstWhileStatementNode statement )
            => throw new NotImplementedException();
    }

    private class MockContinueStatementEvaluator : IContinueStatementEvaluator
    {
        public IAstNode Evaluate( IAstVisitor visitor, AstContinueStatementNode statement )
            => throw new NotImplementedException();
    }
}
