using System.Text;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Interactors.Analysis.Obfuscators.Extensions;
using KSPCompiler.UseCases.Analysis.Evaluations.Declarations;

namespace KSPCompiler.Interactors.Analysis.Obfuscators;

public class CallbackDeclarationEvaluator : ICallbackDeclarationEvaluator
{
    private StringBuilder Output { get; }

    public CallbackDeclarationEvaluator( StringBuilder output )
    {
        Output = output;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstCallbackDeclarationNode node )
    {
        Output.Append( $"on {node.Name}" );

        foreach( var arg in node.ArgumentList.Arguments )
        {
            Output.Append( $" ({arg.Name})" );
        }

        Output.NewLine();

        node.Block.AcceptChildren( visitor );

        Output.Append( "end on" );
        Output.NewLine();

        return node;
    }
}
