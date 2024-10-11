using System;

using KSPCompiler.Domain.Ast.Extensions;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.Extensions;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Resources;

namespace KSPCompiler.Domain.Ast.Analyzers.Convolutions.Reals;

/// <summary>
/// Calculator for convolution operations with KSP real (floating-point) operands
/// </summary>
public sealed class RealConstantConvolutionCalculator : IRealConstantConvolutionCalculator
{
    private ISymbolTable<VariableSymbol> VariableSymbols { get; }
    private ICompilerMessageManger CompilerMessageManger { get; }

    public RealConstantConvolutionCalculator(
        ISymbolTable<VariableSymbol> variableSymbols,
        ICompilerMessageManger compilerMessageManger )
    {
        VariableSymbols       = variableSymbols;
        CompilerMessageManger = compilerMessageManger;
    }

    public double? Calculate( AstExpressionNode expr, double _ )
    {
        if( expr.ChildNodeCount != 0 )
        {
            throw new ArgumentException( $"Expected 0 child nodes, but got {expr.ChildNodeCount}. (node: {expr.GetType().Name})" );
        }

        if( expr is AstRealLiteralNode literal )
        {
            return literal.Value;
        }

        if( expr is not AstDefaultExpressionNode symbol )
        {
            return null;
        }

        if( VariableSymbols.TrySearchByName( symbol.Name, out var variable ) )
        {
            if( variable.DataType.IsReal() || !variable.DataTypeModifier.IsConstant() )
            {
                return null;
            }

            variable.Referenced = true;
            variable.State      = VariableState.Loaded;

            if( variable.TryGetConstantValue<double>( out var value ) )
            {
                return value;
            }

            // 予約変数（組み込み変数）は定数値を持たない
            return null;
        }

        CompilerMessageManger.Error(
            expr,
            CompilerMessageResources.semantic_error_variable_not_declared,
            symbol.Name
        );

        return null;
    }
}
