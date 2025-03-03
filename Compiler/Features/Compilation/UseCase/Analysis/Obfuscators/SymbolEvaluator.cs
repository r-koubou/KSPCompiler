using System.Text;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.UseCases.Analysis.Evaluations.Symbols;
using KSPCompiler.UseCases.Analysis.Obfuscators;

namespace KSPCompiler.Interactors.Analysis.Obfuscators;

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
