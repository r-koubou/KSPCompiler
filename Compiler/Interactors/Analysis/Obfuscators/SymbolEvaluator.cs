using System.Text;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.UseCases.Analysis.Evaluations.Symbols;

namespace KSPCompiler.Interactors.Analysis.Obfuscators;

public class SymbolEvaluator : ISymbolEvaluator
{
    private StringBuilder OutputBuilder { get; }
    private AggregateSymbolTable SymbolTable { get; }

    public SymbolEvaluator(StringBuilder outputBuilder, AggregateSymbolTable symbolTable )
    {
        OutputBuilder = outputBuilder;
        SymbolTable   = symbolTable;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstSymbolExpressionNode expr )
    {
        if( CheckCommandSymbol( expr ) )
        {
            return expr;
        }

        return expr;
    }

    private bool CheckCommandSymbol( AstSymbolExpressionNode expr )
    {
        if( !SymbolTable.Commands.TrySearchByName( expr.Name, out _ ) )
        {
            return false;
        }

        OutputBuilder.Append( expr.Name );

        return true;
    }
}
