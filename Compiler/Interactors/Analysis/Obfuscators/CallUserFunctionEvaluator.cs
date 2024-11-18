using System.Text;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.UseCases.Analysis.Evaluations.UserFunctions;
using KSPCompiler.UseCases.Analysis.Obfuscators;

namespace KSPCompiler.Interactors.Analysis.Obfuscators;

public class CallUserFunctionEvaluator : ICallUserFunctionEvaluator
{
    private StringBuilder Output { get; }
    private IObfuscatedUserFunctionTable ObfuscatedTable { get; }

    public CallUserFunctionEvaluator( StringBuilder output, IObfuscatedUserFunctionTable obfuscatedTable )
    {
        Output          = output;
        ObfuscatedTable = obfuscatedTable;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstCallUserFunctionStatementNode statement )
    {
        var name = ObfuscatedTable.GetObfuscatedByName( statement.Name );

        Output.Append( $"call {name}\n" );

        return statement;
    }
}
