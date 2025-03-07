using System;
using System.Text;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Commands;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Obfuscators;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Obfuscators;

public class MockCallCommandEvaluatorVisitor : DefaultAstVisitor
{
    private StringBuilder Output { get; }
    private ICallCommandEvaluator Evaluator { get; set; } = new NullEvaluator();
    private IObfuscatedVariableTable ObfuscatedTable { get; }

    public MockCallCommandEvaluatorVisitor(
        StringBuilder output,
        IObfuscatedVariableTable obfuscatedTable )
    {
        Output          = output;
        ObfuscatedTable = obfuscatedTable;
    }

    public void Inject( ICallCommandEvaluator evaluator )
    {
        Evaluator = evaluator;
    }

    public override IAstNode Visit( AstSymbolExpressionNode node )
    {
        if( ObfuscatedTable.TryGetObfuscatedByName( node.Name, out var obfuscatedName ) )
        {
            Output.Append( obfuscatedName );
            return node;
        }

        Output.Append( node.Name );
        return node;
    }

    public override IAstNode Visit( AstCallCommandExpressionNode node )
        => Evaluator.Evaluate( this, node );

    private class NullEvaluator : ICallCommandEvaluator
    {
        public IAstNode Evaluate( IAstVisitor visitor, AstCallCommandExpressionNode node )
            => throw new NotImplementedException();
    }
}
