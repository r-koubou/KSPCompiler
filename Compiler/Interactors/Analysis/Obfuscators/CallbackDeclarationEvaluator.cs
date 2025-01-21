using System.Text;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Interactors.Analysis.Obfuscators.Extensions;
using KSPCompiler.UseCases.Analysis.Evaluations.Declarations;
using KSPCompiler.UseCases.Analysis.Obfuscators;

namespace KSPCompiler.Interactors.Analysis.Obfuscators;

public class CallbackDeclarationEvaluator(
    StringBuilder output,
    AggregateObfuscatedSymbolTable obfuscatedSymbolTable
) : ICallbackDeclarationEvaluator
{
    private StringBuilder Output { get; } = output;
    private IObfuscatedVariableTable ObfuscatedVariableTable { get; } = obfuscatedSymbolTable.Variables;

    public IAstNode Evaluate( IAstVisitor visitor, AstCallbackDeclarationNode node )
    {
        Output.Append( $"on {node.Name}" );

        foreach( var arg in node.ArgumentList.Arguments )
        {
            if( ObfuscatedVariableTable.TryGetObfuscatedByName( arg.Name, out var obfuscated ) )
            {
                Output.Append( $" ({obfuscated})" );
            }
            else
            {
                Output.Append( $" ({obfuscated})" );
            }
        }

        Output.NewLine();

        node.Block.AcceptChildren( visitor );

        Output.Append( "end on" );
        Output.NewLine();

        return node;
    }
}
