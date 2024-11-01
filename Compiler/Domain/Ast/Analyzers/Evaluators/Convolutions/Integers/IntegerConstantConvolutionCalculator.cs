using System;

using KSPCompiler.Domain.Ast.Extensions;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.Extensions;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Resources;

namespace KSPCompiler.Domain.Ast.Analyzers.Evaluators.Convolutions.Integers;

/// <summary>
/// Calculator for convolution operations with integer constants
/// </summary>
public sealed class IntegerConstantConvolutionCalculator : IIntegerConstantConvolutionCalculator
{
    private IVariableSymbolTable VariableSymbols { get; }
    private ICompilerMessageManger CompilerMessageManger { get; }

    public IntegerConstantConvolutionCalculator(
        IVariableSymbolTable variableSymbols,
        ICompilerMessageManger compilerMessageManger )
    {
        VariableSymbols       = variableSymbols;
        CompilerMessageManger = compilerMessageManger;
    }

    public int? Calculate( IAstVisitor visitor, AstExpressionNode expr, int workingValueForRecursive )
    {
        if( expr.ChildNodeCount != 0 )
        {
            throw new ArgumentException( $"Expected 0 child nodes, but got {expr.ChildNodeCount}. (node: {expr.GetType().Name})" );
        }

        if( expr is AstIntLiteralNode literal )
        {
            return literal.Value;
        }

        if( VariableSymbols.TrySearchByName( expr.Name, out var variable ) )
        {
            if( !variable.DataType.IsInt() || !variable.Modifier.IsConstant() )
            {
                return null;
            }

            variable.Referenced = true;
            variable.State      = VariableState.Loaded;

            if( variable.TryGetConstantValue<int>( out var value ) )
            {
                return value;
            }

            // 予約変数（組み込み変数）は定数値を持たない
            return null;
        }

        CompilerMessageManger.Error(
            expr,
            CompilerMessageResources.semantic_error_variable_not_declared,
            expr.Name
        );

        return null;
    }
}
