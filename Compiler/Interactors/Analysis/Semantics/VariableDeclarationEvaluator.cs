using KSPCompiler.Domain;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Extensions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.Events;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.Extensions;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Interactors.Analysis.Commons.Evaluations;
using KSPCompiler.Interactors.Analysis.Commons.Extensions;
using KSPCompiler.Interactors.Analysis.Extensions;
using KSPCompiler.Interactors.Analysis.Semantics.Extensions;
using KSPCompiler.Resources;
using KSPCompiler.UseCases.Analysis.Evaluations.Declarations;

namespace KSPCompiler.Interactors.Analysis.Semantics;

public class VariableDeclarationEvaluator : IVariableDeclarationEvaluator
{
    private IEventEmitter EventEmitter { get; }
    private ISymbolTable<VariableSymbol> UserVariables { get; }
    private ISymbolTable<VariableSymbol> BuiltInVariables { get; }
    private ISymbolTable<UITypeSymbol> UITypes { get; }

    public VariableDeclarationEvaluator(
        IEventEmitter eventEmitter,
        ISymbolTable<VariableSymbol> builtInVariables,
        ISymbolTable<VariableSymbol> userVariables,
        ISymbolTable<UITypeSymbol> uiTypes )
    {
        EventEmitter     = eventEmitter;
        BuiltInVariables = builtInVariables;
        UserVariables    = userVariables;
        UITypes          = uiTypes;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstVariableDeclarationNode node )
    {
        // 予約済み（NIが禁止している）接頭語検査
        // TODO: より厳密なチェックをする必要がでたらコメント解除
        // ValidateBuiltInPrefix( node );

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

        if( !UserVariables.Add( variable ) )
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
            EventEmitter.Emit(
                node.AsErrorEvent(
                    CompilerMessageResources.symbol_error_declare_variable_outside,
                    node.Name
                )
            );

            return false;
        }

        if( callback.Name != "init" )
        {
            EventEmitter.Emit(
                node.AsErrorEvent(
                    CompilerMessageResources.symbol_error_declare_variable_outside,
                    node.Name
                )
            );

            return false;
        }

        return true;
    }

    private bool ValidateBuiltInPrefix( AstVariableDeclarationNode node )
    {
        var builtInPrefixValidator = new NonAstVariableNameBuiltInValidator();

        if( !builtInPrefixValidator.Validate( node ) )
        {
            EventEmitter.Emit(
                node.AsWarningEvent(
                    CompilerMessageResources.symbol_error_declare_variable_ni_builtin,
                    node.Name
                )
            );

            return false;
        }

        return true;
    }

    private bool TryCreateNewSymbol( AstVariableDeclarationNode node, out VariableSymbol result )
    {
        var variableType = DataTypeUtility.GuessFromSymbolName( node.Name );

        // 配列型に const は付与できない
        if( variableType.IsArray() && node.Modifier.HasConstant() )
        {
            result = null!;
            EventEmitter.Emit(
                node.AsErrorEvent(
                    CompilerMessageResources.semantic_error_declare_variable_cannot_const,
                    node.Name
                )
            );

            return false;
        }

        // ビルトイン変数との重複
        if( BuiltInVariables.TrySearchByName( node.Name, out result ) )
        {
            EventEmitter.Emit(
                node.AsErrorEvent(
                    CompilerMessageResources.symbol_error_declare_variable_builtin,
                    node.Name
                )
            );

            return false;
        }

        if( !UserVariables.TrySearchByName( node.Name, out result ) )
        {
            // 未定義：新規追加可能
            result = node.As();

            return true;
        }

        // ユーザー変数として宣言済み
        EventEmitter.Emit(
            node.AsErrorEvent(
                CompilerMessageResources.symbol_error_declare_variable_already,
                node.Name
            )
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
        foreach( var modifier in node.Modifier.GetUIModifiers() )
        {
            if( !UITypes.TrySearchByName( modifier, out var uiType ) )
            {
                EventEmitter.Emit(
                    node.AsErrorEvent(
                        CompilerMessageResources.semantic_error_declare_variable_unkown_ui,
                        modifier
                    )
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
        }

        return true;
    }

    #endregion ~Primary validations

    #region Initializer root

    private bool ValidateInitialValue( IAstVisitor visitor, AstVariableDeclarationNode node, VariableSymbol variable )
    {
        // constあり＋初期化代入式が無い場合
        if( variable.Modifier.IsConstant() && node.Initializer.IsNull() )
        {
            EventEmitter.Emit(
                node.AsErrorEvent(
                    CompilerMessageResources.semantic_error_declare_variable_required_initializer,
                    node.Name
                )
            );

            return false;
        }

        // constなし＋初期化代入式がない場合はスキップ
        if( node.Initializer.IsNull() )
        {
            // 例外として文字列型は宣言時に初期化代入はできないので宣言 = 初期化済み としてマーク
            if( variable.DataType.IsString() )
            {
                variable.State = SymbolState.Initialized;
            }

            return true;
        }

        var result = ValidateVariableInitializer( visitor, node, variable );

        // 成功していれば初期化済みとしてマーク
        if( result )
        {
            variable.State = SymbolState.Initialized;
        }

        return result;
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
            EventEmitter.Emit(
                node.AsErrorEvent(
                    CompilerMessageResources.semantic_error_declare_variable_invalid_initializer,
                    node.Name
                )
            );

            return false;
        }

        // 文字列型は宣言時に初期化代入はできない
        if( variable.DataType.IsString() )
        {
            EventEmitter.Emit(
                node.AsErrorEvent(
                    CompilerMessageResources.semantic_error_declare_variable_string_initializer,
                    node.Name
                )
            );

            return false;
        }

        // 初期化代入式が欠落している
        if( initializer.IsNull() || initializer.Expression.IsNull() )
        {
            EventEmitter.Emit(
                node.AsErrorEvent(
                    CompilerMessageResources.semantic_error_declare_variable_required_initializer,
                    node.Name
                )
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
            EventEmitter.Emit(
                node.AsErrorEvent(
                    CompilerMessageResources.semantic_error_declare_variable_noconstant_initializer,
                    node.Name
                )
            );

            return false;
        }

        // 型の一致チェック
        if( !TypeCompatibility.IsAssigningTypeCompatible( variable.DataType, evaluated.TypeFlag ) )
        {
            EventEmitter.Emit(
                node.AsErrorEvent(
                    CompilerMessageResources.semantic_error_assign_type_compatible,
                    variable.DataType.ToMessageString(),
                    evaluated.TypeFlag.ToMessageString()
                )
            );

            return false;
        }

        // 代入値が畳み込みされた値（リテラル値）であれば、右項をその値に置き換える
        if( evaluated.IsLiteralNode() )
        {
            initializer.Expression = evaluated;
        }

        return true;
    }

    #endregion ~Primitive Initializer

    #region Array Initializer

    private bool ValidateArrayInitializer( IAstVisitor visitor, AstVariableDeclarationNode node, VariableSymbol variable )
    {
        var initializer = node.Initializer.ArrayInitializer;

        // 配列型変数に対し、プリミティブ型初期化式を用いている
        if( node.Initializer.PrimitiveInitializer.IsNotNull() )
        {
            EventEmitter.Emit(
                node.AsErrorEvent(
                    CompilerMessageResources.semantic_error_declare_variable_invalid_initializer,
                    node.Name
                )
            );

            return false;
        }

        // 配列要素サイズ・初期化代入式なし
        if( initializer.IsNull() )
        {
            EventEmitter.Emit(
                node.AsErrorEvent(
                    CompilerMessageResources.semantic_error_declare_variable_required_initializer,
                    node.Name
                )
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
            EventEmitter.Emit(
                node.AsErrorEvent(
                    CompilerMessageResources.semantic_error_declare_variable_arraysize,
                    node.Name
                )
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
            EventEmitter.Emit(
                node.AsErrorEvent(
                    CompilerMessageResources.semantic_error_declare_variable_maxarraysize,
                    node.Name,
                    arraySize.Value
                )
            );

            return false;
        }

        // 配列サイズが 0 以下
        if( arraySize.Value <= 0 )
        {
            EventEmitter.Emit(
                node.AsErrorEvent(
                    CompilerMessageResources.semantic_error_declare_variable_arraysize,
                    node.Name
                )
            );

            return false;
        }
        // 初期化要素数が配列サイズより大きい
        if( arraySize.Value < initializer.Initializer.Expressions.Count )
        {
            EventEmitter.Emit(
                node.AsErrorEvent(
                    CompilerMessageResources.semantic_error_declare_variable_arrayinitilizer_sizeover,
                    node.Name
                )
            );

            return false;
        }

        // シンボル情報に配列サイズを反映
        variable.ArraySize = arraySize.Value;

        // 配列サイズが畳み込みされた値を式から置き換える
        initializer.Size = arraySize;

        return true;

    }

    private bool ValidateArrayElements( IAstVisitor visitor, AstVariableDeclarationNode node, VariableSymbol variable, AstArrayInitializerNode initializer )
    {
        var result = true;

        // 初期値代入なし
        if( initializer.Initializer.IsNull() )
        {
            return true;
        }

        // 初期値リストの型チェック
        //foreach( var expr in initializer.Initializer.Expressions )
        for( var i = 0; i < initializer.Initializer.Expressions.Count; i++ )
        {
            var expr = initializer.Initializer.Expressions[ i ];

            if( expr.Accept( visitor ) is not AstExpressionNode evaluated )
            {
                throw new AstAnalyzeException( expr, "Array initializer expression evaluation failed" );
            }

            // リテラル or 定数でないと初期化できない
            if( !evaluated.Constant )
            {
                EventEmitter.Emit(
                    node.AsErrorEvent(
                        CompilerMessageResources.semantic_error_declare_variable_arrayinitilizer_noconstant,
                        variable.Name,
                        i
                    )
                );

                result = false;

                continue;
            }

            // 型の一致チェック
            if( !TypeCompatibility.IsAssigningTypeCompatible( variable.DataType.TypeMasked(), evaluated.TypeFlag ) )
            {
                result = false;

                EventEmitter.Emit(
                    node.AsErrorEvent(
                        CompilerMessageResources.semantic_error_declare_variable_arrayinitilizer_incompatible,
                        variable.Name,
                        i,
                        evaluated.TypeFlag.ToMessageString()
                    )
                );

                continue;
            }

            // 値が畳み込みされた値（リテラル値）であれば、要素の式をその値に置き換える
            if( evaluated.IsLiteralNode() )
            {
                initializer.Initializer.Expressions[ i ] = evaluated;
            }
        }

        return result;
    }

    #endregion ~Array Initializer

    #region UI Initializer

    private bool ValidateUIInitializer( IAstVisitor visitor, AstVariableDeclarationNode node, VariableSymbol variable )
    {
        if( variable.DataType.IsArray() )
        {
            return ValidateArrayBasedUIInitializer( visitor, node, node.Initializer.ArrayInitializer, variable );
        }

        return ValidatePrimitiveBasedUIInitializer( visitor, node, node.Initializer.PrimitiveInitializer.UIInitializer, variable );
    }

    private bool ValidateArrayBasedUIInitializer( IAstVisitor visitor, AstVariableDeclarationNode node, AstArrayInitializerNode initializer, VariableSymbol variable )
    {
        if( !ValidateArraySize( visitor, node, initializer, variable ) )
        {
            return false;
        }

        return ValidateUIArguments( visitor, node, initializer.Initializer, variable.UIType );
    }

    private bool ValidatePrimitiveBasedUIInitializer( IAstVisitor visitor, AstVariableDeclarationNode node, AstExpressionListNode initializer, VariableSymbol variable )
    {
        // パラメータ数が一致しない
        if( initializer.Count != variable.UIType.InitializerArguments.Count )
        {
            EventEmitter.Emit(
                node.AsErrorEvent(
                    CompilerMessageResources.semantic_error_declare_variable_uiinitializer_count_incompatible,
                    node.Name,
                    variable.UIType.InitializerArguments.Count,
                    initializer.Count
                )
            );

            return false;
        }

        return ValidateUIArguments( visitor, node, initializer, variable.UIType );
    }

    private bool ValidateUIArguments( IAstVisitor visitor, AstVariableDeclarationNode node, AstExpressionListNode expressionList, UITypeSymbol uiType )
    {
        if( expressionList.Count != uiType.InitializerArguments.Count )
        {
            EventEmitter.Emit(
                node.AsErrorEvent(
                    CompilerMessageResources.semantic_error_declare_variable_uiinitializer_count_incompatible,
                    node.Name,
                    uiType.InitializerArguments.Count,
                    expressionList.Count
                )
            );

            return false;
        }

        for( var i = 0; i < expressionList.Count; i++ )
        {
            var expr = expressionList.Expressions[ i ];

            if( expr.Accept( visitor ) is not AstExpressionNode evaluated )
            {
                throw new AstAnalyzeException( node, "UI initializer expression evaluation failed" );
            }

            // リテラル or 定数でないと初期化できない
            if( !evaluated.Constant )
            {
                EventEmitter.Emit(
                    node.AsErrorEvent(
                        CompilerMessageResources.semantic_error_declare_variable_uiinitializer_nonconstant,
                        node.Name,
                        i + 1 // 1 origin
                    )
                );

                return false;
            }

            var requiredType = uiType.InitializerArguments[ i ].DataType;

            // 型の一致チェック
            if( !TypeCompatibility.IsTypeCompatible( evaluated.TypeFlag, requiredType ) )
            {
                EventEmitter.Emit(
                    node.AsErrorEvent(
                        CompilerMessageResources.semantic_error_declare_variable_uiinitializer_incompatible,
                        node.Name,
                        i + 1, // 1 origin
                        evaluated.TypeFlag.ToMessageString()
                    )
                );

                return false;
            }

            // 引数が畳み込みでリテラルになっていれば引数の式を置き換える
            if( evaluated.IsLiteralNode() )
            {
                expressionList.Expressions[ i ] = evaluated;
            }
        }

        return true;
    }

    #endregion ~UI Initializer

}
