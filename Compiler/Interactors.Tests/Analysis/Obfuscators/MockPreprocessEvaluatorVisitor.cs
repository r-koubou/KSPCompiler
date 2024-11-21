using System;
using System.Text;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Interactors.Analysis.Obfuscators.Extensions;
using KSPCompiler.UseCases.Analysis.Evaluations.Preprocessing;

namespace KSPCompiler.Interactors.Tests.Analysis.Obfuscators;

public class MockPreprocessEvaluatorVisitor : DefaultAstVisitor
{
    private StringBuilder Output { get; }
    private IPreprocessEvaluator Evaluator { get; set; } = new MockPreprocessEvaluator();

    public MockPreprocessEvaluatorVisitor( StringBuilder output )
    {
        Output = output;
    }

    public void Inject( IPreprocessEvaluator evaluator )
        => Evaluator = evaluator;

    public override IAstNode Visit( AstPreprocessorIfdefineNode node )
        => Evaluator.Evaluate( this, node );

    public override IAstNode Visit( AstPreprocessorIfnotDefineNode node )
        => Evaluator.Evaluate( this, node );

    public override IAstNode Visit( AstSymbolExpressionNode node )
    {
        Output.Append( node.Name );
        return node;
    }

    public override IAstNode Visit( AstCallCommandExpressionNode node )
    {
        node.Left.Accept( this );

        Output.Append( "()" )
              .NewLine();

        return node;
    }

    private class MockPreprocessEvaluator : IPreprocessEvaluator
    {
        public IAstNode Evaluate( IAstVisitor visitor, AstPreprocessorIfdefineNode node )
            => throw new NotImplementedException();

        public IAstNode Evaluate( IAstVisitor visitor, AstPreprocessorIfnotDefineNode node )
            => throw new NotImplementedException();
    }

}
