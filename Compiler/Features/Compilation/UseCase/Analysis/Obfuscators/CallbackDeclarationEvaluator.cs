using System.Text;

using KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Declarations;
using KSPCompiler.Features.Compilation.UseCase.Abstractions.Obfuscators;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators.Extensions;
using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Ast.Nodes.Blocks;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators;

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
