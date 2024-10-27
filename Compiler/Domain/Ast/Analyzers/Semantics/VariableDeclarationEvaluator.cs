using System;

using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Declarations;
using KSPCompiler.Domain.Ast.Extensions;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Extensions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.Extensions;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Resources;

namespace KSPCompiler.Domain.Ast.Analyzers.Semantics;

public class VariableDeclarationEvaluator : IVariableDeclarationEvaluator
{
    private ICompilerMessageManger CompilerMessageManger { get; }
    private ISymbolTable<VariableSymbol> VariableSymbols { get; }
    private ISymbolTable<UITypeSymbol> UITypeSymbols { get; }

    public VariableDeclarationEvaluator(
        ICompilerMessageManger compilerMessageManger,
        ISymbolTable<VariableSymbol> variableSymbols,
        ISymbolTable<UITypeSymbol> uiTypeSymbols )
    {
        CompilerMessageManger = compilerMessageManger;
        VariableSymbols       = variableSymbols;
        UITypeSymbols         = uiTypeSymbols;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstVariableDeclarationNode node )
    {
        // 予約済み（NIが禁止している）接頭語検査
        ValidateNiReservedPrefix( node );

        // on init 外での変数宣言はエラー
        if( !ValidateCallbackNode( node ) )
        {
            return node;
        }

        // 定義済みの検査
        if( !TryCreateNewSymbol( node, out var variable ) )
        {
            return node;
        }

        // UI型の検査
        if( !ValidateUIType( node, variable ) )
        {
            return node;
        }

        if( !ValidateInitialValue( visitor, node, variable ) )
        {
            return node;
        }

        if( !VariableSymbols.Add( variable ) )
        {
            throw new AstAnalyzeException( this, node, $"Variable symbol add failed {variable.Name}" );
        }

        return node;
    }

    #region Primary validations

    private bool ValidateCallbackNode( AstVariableDeclarationNode node )
    {
        if( !node.TryGetParent<AstCallbackDeclarationNode>( out var callback ) )
        {
            CompilerMessageManger.Error(
                node,
                CompilerMessageResources.symbol_error_declare_variable_outside,
                node.Name
            );

            return false;
        }

        if( callback.Name != "init" )
        {
            CompilerMessageManger.Error(
                node,
                CompilerMessageResources.symbol_error_declare_variable_outside,
                node.Name
            );

            return false;
        }

        return true;
    }

    private bool ValidateNiReservedPrefix( AstVariableDeclarationNode node )
    {
        var reservedPrefixValidator = new NonAstVariableNamePrefixReservedValidator();

        if( !reservedPrefixValidator.Validate( node ) )
        {
            CompilerMessageManger.Error(
                node,
                CompilerMessageResources.symbol_error_declare_variable_ni_reserved,
                node.Name
            );

            return false;
        }

        return true;
    }

    private bool TryCreateNewSymbol( AstVariableDeclarationNode node, out VariableSymbol result )
    {
        var variableType = DataTypeUtility.GuessFromSymbolName( node.Name );

        // 配列型に const は付与できない
        if( variableType.IsArray() && node.Modifier == "const" )
        {
            result = null!;
            CompilerMessageManger.Error(
                node,
                CompilerMessageResources.semantic_error_declare_variable_cannot_const,
                node.Name
            );

            return false;
        }

        if( !VariableSymbols.TrySearchByName( node.Name, out result ) )
        {
            // 未定義：新規追加可能
            result = node.As();

            return true;
        }

        // NI の予約変数との重複
        if( result.Reserved )
        {
            CompilerMessageManger.Error(
                node,
                CompilerMessageResources.symbol_error_declare_variable_reserved,
                node.Name
            );

            return false;
        }

        // ユーザー変数として宣言済み
        CompilerMessageManger.Error(
            node,
            CompilerMessageResources.symbol_error_declare_variable_already,
            node.Name
        );

        return false;
    }

    private bool ValidateUIType( AstVariableDeclarationNode node, VariableSymbol variable )
    {
        if( !variable.Modifier.IsUI() )
        {
            return true;
        }

        // 有効な UI 型かチェック
        if( !UITypeSymbols.TrySearchByName( node.Modifier, out var uiType ) )
        {
            CompilerMessageManger.Warning(
                node,
                CompilerMessageResources.symbol_error_declare_variable_unkown,
                node.Modifier
            );

            return false;
        }

        // そのUI型は後から変更不可能な仕様の場合
        if( uiType.Modifier.IsConstant() )
        {
            variable.Modifier |= ModifierFlag.Const;
        }

        // UI型情報を参照
        // 意味解析時に使用するため、用意されている変数に保持
        variable.UIType = uiType;

        return true;
    }

    #endregion ~Primary validations

    #region Initializer root

    private bool ValidateInitialValue( IAstVisitor visitor, AstVariableDeclarationNode node, VariableSymbol variable )
    {
        // constあり＋初期化代入式が無い場合
        if( variable.Modifier.IsConstant() && node.Initializer.IsNull() )
        {
            CompilerMessageManger.Error(
                node,
                CompilerMessageResources.semantic_error_declare_variable_required_initializer,
                node.Name
            );
            return false;
        }

        // constなし＋初期化代入式がない場合はスキップ
        if( node.Initializer.IsNull() )
        {
            return true;
        }

        return ValidateVariableInitializer( visitor, node, variable );
    }

    private bool ValidateVariableInitializer( IAstVisitor visitor, AstVariableDeclarationNode node, VariableSymbol variable )
    {
        if( variable.Modifier.IsUI() )
        {
            return ValidateUIInitializer( visitor, node, variable );
        }
        else if( variable.DataType.IsArray() )
        {
            return ValidateArrayInitializer( visitor, node, variable );
        }

        return ValidatePrimitiveInitializer( visitor, node, variable );
    }

    #endregion ~Initializer root

    #region Primitive Initializer

    private bool ValidatePrimitiveInitializer( IAstVisitor visitor, AstVariableDeclarationNode node, VariableSymbol variable )
    {
        var initializer = node.Initializer.PrimitiveInitializer;

        // プリミティブ型変数に対し、配列初期化式を用いている
        if( node.Initializer.ArrayInitializer.IsNotNull() )
        {
            CompilerMessageManger.Error(
                node,
                CompilerMessageResources.semantic_error_declare_variable_invalid_initializer,
                node.Name
            );

            return false;
        }

        // 文字列型は宣言時に初期化代入はできない
        if( variable.DataType.IsString() )
        {
            CompilerMessageManger.Error(
                node,
                CompilerMessageResources.semantic_error_declare_variable_string_initializer,
                node.Name
            );

            return false;
        }

        // 初期化代入式が欠落している
        if( initializer.IsNull() || initializer.Expression.IsNull() )
        {
            CompilerMessageManger.Error(
                node,
                CompilerMessageResources.semantic_error_declare_variable_required_initializer,
                node.Name
            );

            return false;
        }

        if( initializer.Expression.Accept( visitor ) is not AstExpressionNode evaluated )
        {
            throw new AstAnalyzeException( initializer, "Primitive initializer expression evaluation failed" );
        }

        // リテラル or 定数でないと初期化できない
        if( !evaluated.Constant )
        {
            CompilerMessageManger.Error(
                node,
                CompilerMessageResources.semantic_error_declare_variable_noconstant_initializer,
                node.Name
            );

            return false;
        }

        // 型の一致チェック
        if( AssigningTypeUtility.IsTypeCompatible( variable.DataType, evaluated.TypeFlag ) )
        {
            return true;
        }

        CompilerMessageManger.Error(
            node,
            CompilerMessageResources.semantic_error_assign_type_compatible,
            variable.DataType.ToMessageString(),
            evaluated.TypeFlag.ToMessageString()
        );

        return false;

    }

    #endregion ~Primitive Initializer

    #region Array Initializer

    private bool ValidateArrayInitializer( IAstVisitor visitor, AstVariableDeclarationNode node, VariableSymbol variable )
    {
        var initializer = node.Initializer.ArrayInitializer;

        // 配列型変数に対し、プリミティブ型初期化式を用いている
        if( node.Initializer.PrimitiveInitializer.IsNotNull() )
        {
            CompilerMessageManger.Error(
                node,
                CompilerMessageResources.semantic_error_declare_variable_invalid_initializer,
                node.Name
            );

            return false;
        }

        // 配列要素サイズ・初期化代入式なし
        if( initializer.IsNull() )
        {
            CompilerMessageManger.Error(
                node,
                CompilerMessageResources.semantic_error_declare_variable_required_initializer,
                node.Name
            );

            return false;
        }

        if( !ValidateArraySize( visitor, node, initializer, variable ) )
        {
            return false;
        }

        // 配列要素の型チェック
        return ValidateArrayElements( visitor, node, variable, initializer );
    }

    private bool ValidateArraySize( IAstVisitor visitor, AstVariableDeclarationNode node, AstArrayInitializerNode initializer, VariableSymbol variable )
    {
        // 配列サイズのチェック
        if( initializer.Size.Accept( visitor ) is not AstExpressionNode arraySizeExpr )
        {
            throw new AstAnalyzeException( initializer.Size, "Array size expression evaluation failed" );
        }

        // リテラル or 定数でないと初期化できない
        if( !arraySizeExpr.Constant )
        {
            CompilerMessageManger.Error(
                node,
                CompilerMessageResources.semantic_error_declare_variable_arraysize,
                node.Name
            );

            return false;
        }

        if( arraySizeExpr is not AstIntLiteralNode arraySize )
        {
            throw new AstAnalyzeException( arraySizeExpr, "Array size expression is not integer" );
        }

        // 配列サイズが上限を超えている
        if( arraySize.Value >= KspLanguageLimitations.MaxArraySize )
        {
            CompilerMessageManger.Error(
                node,
                CompilerMessageResources.semantic_error_declare_variable_maxarraysize,
                node.Name,
                arraySize.Value
            );

            return false;
        }

        // 配列サイズが 0 以下
        if( arraySize.Value <= 0 )
        {
            CompilerMessageManger.Error(
                node,
                CompilerMessageResources.semantic_error_declare_variable_arraysize,
                node.Name
            );

            return false;
        }
        // 初期化要素数が配列サイズより大きい
        if( arraySize.Value < initializer.Initializer.Expressions.Count )
        {
            CompilerMessageManger.Error(
                node,
                CompilerMessageResources.semantic_error_declare_variable_arrayinitilizer_sizeover,
                node.Name
            );

            return false;
        }

        // シンボル情報に配列サイズを反映
        variable.ArraySize = arraySize.Value;

        return true;

    }

    private bool ValidateArrayElements( IAstVisitor visitor, AstVariableDeclarationNode node, VariableSymbol variable, AstArrayInitializerNode initializer )
    {
        var result = true;
        var i = -1;

        // 初期値代入なし
        if( initializer.Initializer.IsNull() )
        {
            return true;
        }

        // 初期値リストの型チェック
        foreach( var expr in initializer.Initializer.Expressions )
        {
            i++;

            if( expr.Accept( visitor ) is not AstExpressionNode evaluated )
            {
                throw new AstAnalyzeException( expr, "Array initializer expression evaluation failed" );
            }

            // リテラル or 定数でないと初期化できない
            if( !evaluated.Constant )
            {
                CompilerMessageManger.Error(
                    node,
                    CompilerMessageResources.semantic_error_declare_variable_arrayinitilizer_noconstant,
                    variable.Name,
                    i
                );

                result = false;

                continue;
            }

            // 型の一致チェック
            if( AssigningTypeUtility.IsTypeCompatible( variable.DataType.TypeMasked(), evaluated.TypeFlag ) )
            {
                continue;
            }

            result = false;

            CompilerMessageManger.Error(
                node,
                CompilerMessageResources.semantic_error_declare_variable_arrayinitilizer_incompatible,
                variable.Name,
                i,
                evaluated.TypeFlag.ToMessageString()
            );
        }

        return result;
    }

    #endregion ~Array Initializer

    #region UI Initializer

    private bool ValidateUIInitializer( IAstVisitor visitor, AstVariableDeclarationNode node, VariableSymbol variable )
    {
        if( variable.DataType.IsArray() )
        {
            if( !ValidateArrayBasedUIInitializer( visitor, node, node.Initializer.ArrayInitializer, variable ) )
            {
                return false;
            }
        }

        return ValidatePrimitiveBasedUIInitializer( visitor, node, node.Initializer.PrimitiveInitializer, variable );
    }

    private bool ValidateArrayBasedUIInitializer( IAstVisitor visitor, AstVariableDeclarationNode node, AstArrayInitializerNode initializer, VariableSymbol variable )
    {
        if( !ValidateArraySize( visitor, node, initializer, variable ) )
        {
            return false;
        }

        return ValidateUIArguments( visitor, node, initializer.Initializer, variable.UIType );
    }

    private bool ValidatePrimitiveBasedUIInitializer( IAstVisitor visitor, AstVariableDeclarationNode node, AstPrimitiveInitializerNode initializer, VariableSymbol variable )
    {
        var expressions = initializer.UIInitializer;

        // パラメータ数が一致しない
        if( expressions.Count != variable.UIType.InitializerArguments.Count )
        {
            CompilerMessageManger.Error(
                node,
                CompilerMessageResources.semantic_error_declare_variable_uiinitializer_count_incompatible,
                node.Name,
                variable.UIType.InitializerArguments.Count,
                expressions.Count
            );

            return false;
        }

        return ValidateUIArguments( visitor, node, expressions, variable.UIType );
    }

    private bool ValidateUIArguments( IAstVisitor visitor, AstVariableDeclarationNode node, AstExpressionListNode expressionList, UITypeSymbol uiType )
    {
        for(var i = 0; i < expressionList.Count; i++)
        {
            var expr = expressionList.Expressions[ i ];

            if( expr.Accept( visitor ) is not AstExpressionNode evaluated )
            {
                throw new AstAnalyzeException( node, "UI initializer expression evaluation failed" );
            }

            // リテラル or 定数でないと初期化できない
            if( !evaluated.Constant )
            {
                CompilerMessageManger.Error(
                    node,
                    CompilerMessageResources.semantic_error_declare_variable_uiinitializer_nonconstant,
                    node.Name,
                    i + 1 // 1 origin
                );

                return false;
            }

            var requiredType = uiType.InitializerArguments[ i ].DataType;

            // 型の一致チェック
            if( AssigningTypeUtility.IsTypeCompatible( evaluated.TypeFlag, requiredType ) )
            {
                continue;
            }

            CompilerMessageManger.Error(
                node,
                CompilerMessageResources.semantic_error_declare_variable_uiinitializer_incompatible,
                node.Name,
                i + 1, // 1 origin
                evaluated.TypeFlag.ToMessageString()
            );

            return false;
        }

        return true;
    }

    #endregion ~UI Initializer

}
