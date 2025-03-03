using System.Text;

using KSPCompiler.Features.Compilation.Domain.Ast.Nodes;
using KSPCompiler.Features.Compilation.Domain.Ast.Nodes.Statements;
using KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.UserFunctions;
using KSPCompiler.Features.Compilation.UseCase.Abstractions.Obfuscators;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators.Extensions;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators;

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
