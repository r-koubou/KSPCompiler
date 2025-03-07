using System.Text;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Symbols;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Obfuscators;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;
using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators;

public class SymbolEvaluator(
    StringBuilder output,
    AggregateSymbolTable symbolTable,
    AggregateObfuscatedSymbolTable obfuscatedSymbolTable )
    : ISymbolEvaluator
{
    private StringBuilder Output { get; } = output;
    private AggregateSymbolTable SymbolTable { get; } = symbolTable;
    private IObfuscatedVariableTable ObfuscatedVariableTable { get; } = obfuscatedSymbolTable.Variables;

    public IAstNode Evaluate( IAstVisitor visitor, AstSymbolExpressionNode expr )
    {
        if( CheckVariableSymbol( expr ) )
        {
            return expr;
        }

        if( CheckCommandSymbol( expr ) )
        {
            return expr;
        }

        return expr;
    }

    private bool CheckVariableSymbol( AstSymbolExpressionNode expr )
    {
        if( ObfuscatedVariableTable.TryGetObfuscatedByName( expr.Name, out var obfuscated ) )
        {
            Output.Append( obfuscated );
            return true;
        }

        Output.Append( expr.Name );

        return true;
    }

    private bool CheckCommandSymbol( AstSymbolExpressionNode expr )
    {
        if( !SymbolTable.Commands.TrySearchByName( expr.Name, out _ ) )
        {
            return false;
        }

        Output.Append( expr.Name );

        return true;
    }
}
