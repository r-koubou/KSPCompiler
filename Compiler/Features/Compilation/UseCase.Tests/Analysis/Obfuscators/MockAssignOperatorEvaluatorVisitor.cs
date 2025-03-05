using System;
using System.Text;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Operators;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Obfuscators;
using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Obfuscators;

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
