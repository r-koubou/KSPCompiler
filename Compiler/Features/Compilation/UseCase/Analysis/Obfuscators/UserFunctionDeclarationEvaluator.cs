using System.Text;

using KSPCompiler.Features.Compilation.Domain.Ast.Nodes;
using KSPCompiler.Features.Compilation.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Declarations;
using KSPCompiler.Features.Compilation.UseCase.Abstractions.Obfuscators;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators.Extensions;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators;

public class UserFunctionDeclarationEvaluator(
    StringBuilder output,
    AggregateObfuscatedSymbolTable obfuscatedTable )
    : IUserFunctionDeclarationEvaluator
{
    private StringBuilder Output { get; } = output;
    private IObfuscatedUserFunctionTable ObfuscatedTable { get; } = obfuscatedTable.UserFunctions;

    public IAstNode Evaluate( IAstVisitor visitor, AstUserFunctionDeclarationNode node )
    {
        var name = ObfuscatedTable.GetObfuscatedByName( node.Name );

        Output.Append( $"function {name}" ).NewLine();

        node.Block.AcceptChildren( visitor );

        Output.Append( "end function" ).NewLine();

        return node;
    }
}
