using System;
using System.Text;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.UseCases.Analysis.Evaluations.Operators;
using KSPCompiler.UseCases.Analysis.Obfuscators;

namespace KSPCompiler.Interactors.Tests.Analysis.Obfuscators;

public class MockAssignOperatorEvaluatorVisitor : DefaultAstVisitor
{
    private StringBuilder Output { get; }
    private IObfuscatedVariableTable ObfuscatedTable { get; }
    private IAssignOperatorEvaluator Evaluator { get; set; } = new MockAssignOperatorEvaluator();

    public MockAssignOperatorEvaluatorVisitor( StringBuilder output, IObfuscatedVariableTable obfuscatedTable )
    {
        Output          = output;
        ObfuscatedTable = obfuscatedTable;
    }

    public void Inject( IAssignOperatorEvaluator evaluator )
        => Evaluator = evaluator;

    public override IAstNode Visit( AstSymbolExpressionNode node )
    {
        Output.Append( ObfuscatedTable.GetObfuscatedByName( node.Name ) );
        return node;
    }

    public override IAstNode Visit( AstIntLiteralNode node )
    {
        Output.Append( $"{node.Value}" );
        return node;
    }

    public override IAstNode Visit( AstAssignmentExpressionNode node )
        => Evaluator.Evaluate( this, node );

    private class MockAssignOperatorEvaluator :  IAssignOperatorEvaluator
    {
        public IAstNode Evaluate( IAstVisitor visitor, AstExpressionNode expr )
            => throw new NotImplementedException();
    }
}
