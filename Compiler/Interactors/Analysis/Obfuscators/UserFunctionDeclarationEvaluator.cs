using System.Text;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Interactors.Analysis.Obfuscators.Extensions;
using KSPCompiler.UseCases.Analysis.Evaluations.Declarations;
using KSPCompiler.UseCases.Analysis.Obfuscators;

namespace KSPCompiler.Interactors.Analysis.Obfuscators;

public class UserFunctionDeclarationEvaluator : IUserFunctionDeclarationEvaluator
{
    private StringBuilder Output { get; }
    private IObfuscatedUserFunctionTable ObfuscatedTable { get; }

    public UserFunctionDeclarationEvaluator(
        StringBuilder output,
        IObfuscatedUserFunctionTable obfuscatedTable )
    {
        Output          = output;
        ObfuscatedTable = obfuscatedTable;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstUserFunctionDeclarationNode node )
    {
        var name = ObfuscatedTable.GetObfuscatedByName( node.Name );

        Output.Append( $"function {name}" ).NewLine();

        node.Block.AcceptChildren( visitor );

        Output.Append( "end function" ).NewLine();

        return node;
    }
}
