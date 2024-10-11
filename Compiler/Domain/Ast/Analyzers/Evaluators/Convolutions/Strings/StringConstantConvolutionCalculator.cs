using System;
using System.Globalization;

using KSPCompiler.Domain.Ast.Extensions;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.Extensions;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Resources;

namespace KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Strings;

/// <summary>
/// Calculator for convolution operations with string constants
/// </summary>
public sealed class StringConstantConvolutionCalculator : IStringConstantConvolutionCalculator
{
    private ISymbolTable<VariableSymbol> VariableSymbols { get; }
    private ICompilerMessageManger CompilerMessageManger { get; }

    public StringConstantConvolutionCalculator(
        ISymbolTable<VariableSymbol> variableSymbols,
        ICompilerMessageManger compilerMessageManger )
    {
        VariableSymbols       = variableSymbols;
        CompilerMessageManger = compilerMessageManger;
    }
    public string? Calculate( AstExpressionNode expr, string _ )
    {
        if( expr.ChildNodeCount != 0 )
        {
            throw new ArgumentException( $"Expected 0 child nodes, but got {expr.ChildNodeCount}. (node: {expr.GetType().Name})" );
        }

        if( TryGetLiteralValue( expr, out var literalValue ) )
        {
            return literalValue;
        }

        if( TryGetConstantVariable( expr, out var result ) )
        {
            return result;
        }

        CompilerMessageManger.Error(
            expr,
            CompilerMessageResources.semantic_error_variable_not_declared,
            expr.Name
        );

        return null;
    }

    private static bool TryGetLiteralValue( AstExpressionNode expr, out string? result )
    {
        switch( expr )
        {
            case AstStringLiteralNode literal:
                result = literal.Value;
                return true;
            case AstIntLiteralNode intLiteral:
                result = intLiteral.Value.ToString();
                return true;
            case AstRealLiteralNode realLiteral:
                result = realLiteral.Value.ToString( CultureInfo.InvariantCulture );
                return true;
            default:
                result = null;
                return false;
        }
    }

    private bool TryGetConstantVariable( AstExpressionNode symbol, out string? result )
    {

        if( !VariableSymbols.TrySearchByName( symbol.Name, out var variable ) )
        {
            result = null;
            return false;
        }

        if( !variable.DataTypeModifier.IsConstant() )
        {
            result = null;
            return false;
        }

        variable.Referenced = true;
        variable.State      = VariableState.Loaded;

        if( variable.TryGetConstantValue<string>( out var value ) )
        {
            result = value;
            return true;
        }
        else if( variable.TryGetConstantValue<int>( out var intValue ) )
        {
            result = intValue.ToString();
            return true;
        }
        else if( variable.TryGetConstantValue<double>( out var realValue ) )
        {
            result = realValue.ToString( CultureInfo.InvariantCulture );
            return true;
        }

        // 予約変数（組み込み変数）は定数値を持たない
        result = null;
        return false;

    }
}
