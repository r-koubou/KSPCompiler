using System;

using KSPCompiler.Domain.Ast.Extensions;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Resources;

namespace KSPCompiler.Domain.Ast.Analyzers.Convolutions;

/// <summary>
/// Calculator for convolution operations with integer operands
/// </summary>
public sealed class IntegerOperandConvolutionCalculator : IPrimitiveConvolutionOperandCalculator<int>
{
    private ISymbolTable<VariableSymbol> VariableSymbols { get; }
    private ICompilerMessageManger CompilerMessageManger { get; }

    public IntegerOperandConvolutionCalculator(
        ISymbolTable<VariableSymbol> variableSymbols,
        ICompilerMessageManger compilerMessageManger )
    {
        VariableSymbols       = variableSymbols;
        CompilerMessageManger = compilerMessageManger;
    }

    public int? Calculate( AstExpressionSyntaxNode expr, int workingValueForRecursive )
    {
        if( expr.ChildNodeCount != 0 )
        {
            throw new ArgumentException( $"Expected 0 child nodes, but got {expr.ChildNodeCount}. (node: {expr.GetType().Name})" );
        }

        if( expr is AstIntLiteral literal )
        {
            return literal.Value;
        }

        if( expr is not AstSymbolExpression symbol )
        {
            return null;
        }

        if( VariableSymbols.TrySearchByName( symbol.Name, out var variable ) )
        {
            if( variable.DataType.IsInt() || !variable.DataTypeModifier.IsConstant() )
            {
                return null;
            }

            variable.Referenced = true;
            variable.State      = VariableState.Loaded;

            if( variable.Value is int value )
            {
                return value;
            }

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
