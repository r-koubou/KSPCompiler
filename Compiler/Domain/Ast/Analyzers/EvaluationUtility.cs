using System;

using KSPCompiler.Domain.Ast.Extensions;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Resources;

namespace KSPCompiler.Domain.Ast.Analyzers;

public static class EvaluationUtility
{
    public static AstSymbolExpression EvalBinaryOperator<TNode>(
        AstExpressionSyntaxNode node,
        IAstVisitor<TNode> astVisitor,
        ISymbolTable<VariableSymbol> variableTable,
        ICompilerMessageManger compilerMessageManger,
        AbortTraverseToken abortTraverseToken )
        where TNode : IAstNode
    {
        /*
                    <operator>
                        +
                        |
                   +----+----+
                   |         |
            <left-expr>   <right-expr>
        */

        if( node.Left.Accept( astVisitor, abortTraverseToken ) is not AstSymbolExpression left )
        {
            throw new AstAnalyzeException( astVisitor, node, $"Left side of expression is invalid" );
        }

        if( node.Right.Accept( astVisitor, abortTraverseToken ) is not AstSymbolExpression right )
        {
            throw new AstAnalyzeException( astVisitor, node, $"Right side of expression is invalid" );
        }

        if( abortTraverseToken.Aborted )
        {
            return AstSymbolExpression.Null;
        }

        var leftType = left.TypeFlag;
        var rightType = right.TypeFlag;
        var typeCheckResult = true;
        var resultType = DataTypeFlag.TypeVoid;

        if( leftType.IsInt() && rightType.IsInt() )
        {
            resultType = DataTypeFlag.TypeInt;
        }
        else if( leftType.IsReal() && rightType.IsReal() )
        {
            resultType = DataTypeFlag.TypeReal;
        }
        else
        {
            typeCheckResult = false;
        }

        //--------------------------------------------------------------------------
        // 型チェック失敗
        //--------------------------------------------------------------------------
        if( !typeCheckResult )
        {
            compilerMessageManger.Error(
                node,
                CompilerMessageResources.semantic_error_binaryoprator_compatible,
                leftType.ToMessageString(),
                rightType.ToMessageString()
            );

            abortTraverseToken.Abort();
            return AstSymbolExpression.Null;
        }

        var result = new AstSymbolExpression( node )
        {
            TypeFlag = resultType
        };

        //--------------------------------------------------------------------------
        // 左辺、右辺共にリテラル、定数なら式の結果に定数フラグを反映
        // このノード自体を式からリテラルに置き換える
        //--------------------------------------------------------------------------
        if( !left.Reserved && !right.Reserved &&
            !left.TypeFlag.IsArray() && !right.TypeFlag.IsArray() &&
            left.IsConstant && right.IsConstant )
        {
            // TODO 畳み込み
            object? convoluted = 0;

            if( resultType.IsInt() )
            {
                convoluted = EvalConstantIntValue( node, 0, variableTable, compilerMessageManger );

                if( convoluted is not int value )
                {
                    throw new InvalidOperationException("Invalid constant value");
                }

                return new AstIntLiteral( value );
            }
            else if( resultType.IsReal() )
            {
                //constValue = EvalBinaryOperatorReal( node.Operator, left, right );
                result = new AstRealLiteral( (double)convoluted );
            }
        }

        return result;
    }

    #region  Convoluted Evaluation

    /// <summary>
    /// Convloved evaluation of binary operator for integer type.
    /// </summary>
    /// <param name="expr">Expression node</param>
    /// <param name="workingValueForRecursive">Working value for recursive evaluation. First call should be 0.</param>
    /// <param name="variableTable">Variable table</param>
    /// <returns>Evaluated value. If constant value is not found, returns null.</returns>
    static public int? EvalConstantIntValue(
        AstExpressionSyntaxNode expr,
        int? workingValueForRecursive,
        ISymbolTable<VariableSymbol> variableTable,
        ICompilerMessageManger compilerMessageManger )
    {
        //--------------------------------------------------------------------------
        // リテラル・変数
        //--------------------------------------------------------------------------
        if( expr.ChildNodeCount == 0 )
        {
            if( expr is AstIntLiteral intLiteral )
            {
                return intLiteral.Value;
            }

            if( expr is AstSymbolExpression symbol )
            {
                if( variableTable.TrySearchByName( symbol.Name, out var variable ) )
                {
                    if( !variable.DataType.IsInt() || !variable.DataTypeModifier.IsConstant() )
                    {
                        return null;
                    }
                    variable.Referenced = true;
                    variable.State      = VariableState.Loaded;

                    if( variable.Value is int value )
                    {
                        return value;
                    }
                }
                else
                {
                    // TODO variable not found error message
                }
            }
        }
        //--------------------------------------------------------------------------
        // ２項演算子
        //--------------------------------------------------------------------------
        else if( expr.ChildNodeCount == 2 )
        {
            var left = expr.Left;
            var right = expr.Right;

            var numL = EvalConstantIntValue( left, workingValueForRecursive, variableTable, compilerMessageManger );
            if( numL == null )
            {
                return null;
            }

            var numR = EvalConstantIntValue( right, numL, variableTable, compilerMessageManger );
            if( numR == null )
            {
                return null;
            }

            return expr.Id switch
            {
                AstNodeId.Addition    => numL + numR,
                AstNodeId.Subtraction => numL - numR,
                AstNodeId.Multiplying => numL * numR,
                AstNodeId.Division    => numL / numR,
                AstNodeId.Modulo      => numL % numR,
                AstNodeId.BitwiseOr   => numL | numR,
                AstNodeId.BitwiseAnd  => numL & numR,
                _ => null
            };
        }
        //--------------------------------------------------------------------------
        // 単項演算子
        //--------------------------------------------------------------------------
        else if( expr.ChildNodeCount == 1 )
        {
            var num = EvalConstantIntValue( expr.Left, workingValueForRecursive, variableTable, compilerMessageManger );
            if( num == null )
            {
                return null;
            }

            return expr.Id switch
            {
                AstNodeId.UnaryMinus      => -num,
                AstNodeId.UnaryNot        => ~num,
                AstNodeId.UnaryLogicalNot => num == 0 ? 0 : 1, // ここでは 0=false, else=true としている
                _ => null
            };
        }
        return null;
    }

    #endregion
}
