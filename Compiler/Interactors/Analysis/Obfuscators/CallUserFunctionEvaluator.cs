using System.Text;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Interactors.Analysis.Obfuscators.Extensions;
using KSPCompiler.UseCases.Analysis.Evaluations.UserFunctions;
using KSPCompiler.UseCases.Analysis.Obfuscators;

namespace KSPCompiler.Interactors.Analysis.Obfuscators;

public class CallUserFunctionEvaluator( StringBuilder output, AggregateObfuscatedSymbolTable obfuscatedTable )
    : ICallUserFunctionEvaluator
{
    private StringBuilder Output { get; } = output;
    private IObfuscatedUserFunctionTable ObfuscatedTable { get; } = obfuscatedTable.UserFunctions;

    public IAstNode Evaluate( IAstVisitor visitor, AstCallUserFunctionStatementNode statement )
    {
        var name = ObfuscatedTable.GetObfuscatedByName( statement.Symbol.Name );

        Output.Append( $"call {name}" ).NewLine();

        return statement;
    }
}
