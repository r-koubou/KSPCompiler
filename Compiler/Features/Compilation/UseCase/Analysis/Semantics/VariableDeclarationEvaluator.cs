using KSPCompiler.Features.Compilation.Domain;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Declarations;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Evaluations;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Extensions;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Extensions;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics.Extensions;
using KSPCompiler.Resources;
using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Shared.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Shared.Domain.Ast.Nodes.Extensions;
using KSPCompiler.Shared.Domain.Ast.Nodes.Statements;
using KSPCompiler.Shared.Domain.Symbols;
using KSPCompiler.Shared.Domain.Symbols.Extensions;
using KSPCompiler.Shared.Domain.Symbols.MetaData;
using KSPCompiler.Shared.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Shared.EventEmitting;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics;

public class VariableDeclarationEvaluator : IVariableDeclarationEvaluator
{
    private IEventEmitter EventEmitter { get; }
    private AggregateSymbolTable SymbolTables { get; }

    public VariableDeclarationEvaluator(
        IEventEmitter eventEmitter,
        AggregateSymbolTable symbolTables )
    {
        EventEmitter = eventEmitter;
        SymbolTables = symbolTables;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstVariableDeclarationNode node )
    {
        var variable = node.As();

        //
        // 以降の解析を継続させるため、誤りがあっても処理を続行してシンボルテーブルに追加させる
        //

        // 予約済み（NIが禁止している）接頭語検査
        // TODO: 厳密なチェックが必要になったらコメント解除
        // ValidateBuiltInPrefix( node );

        // on init 外での変数宣言はエラー
        ValidateCallbackNode( node );

        // 変数名の検査
        ValidateVariableName( node, variable );

        // 定義済みの検査
        CanCreateNewSymbol( node );

        // UI型の検査
        ValidateUIType( node, variable );

        // 初期化式の検査
        ValidateInitialValue( visitor, node, variable );

        // シンボルテーブルに追加
        if( SymbolTables.UserVariables.Add( variable ) )
        {
            // 成功時（宣言の初回登場）のみ位置情報を設定
            variable.DefinedPosition = node.VariableNamePosition;
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
                node.AsErrorEvent(
                    CompilerMessageResources.symbol_error_declare_variable_ni_builtin,
                    node.Name
                )
            );

            return false;
        }

        return true;
    }

    private bool CanCreateNewSymbol( AstVariableDeclarationNode node )
    {
        var variableType = DataTypeUtility.GuessFromSymbolName( node.Name );

        // 配列型に const は付与できない
        if( variableType.IsArray() && node.Modifier.HasConstant() )
        {
            EventEmitter.Emit(
                node.AsErrorEvent(
                    CompilerMessageResources.semantic_error_declare_variable_cannot_const,
                    node.Name
                )
            );

            return false;
        }

        if( !SymbolTables.TrySearchVariableByName( node.Name, out _ ) )
        {
            // 未定義：新規追加可能
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

    private bool ValidateVariableName( AstVariableDeclarationNode node, VariableSymbol symbol )
    {
        // NI の予約変数との重複
        if( symbol.BuiltIn )
        {
            EventEmitter.Emit(
                node.AsErrorEvent(
                    CompilerMessageResources.symbol_error_declare_variable_builtin,
                    node.Name
                )
            );

            return false;
        }

        return true;
    }

    private void ValidateUIType( AstVariableDeclarationNode node, VariableSymbol variable )
    {
        if( !variable.Modifier.IsUI() )
        {
            return;
        }

        // 有効な UI 型かチェック
        foreach( var modifier in node.Modifier.GetUIModifiers() )
        {
            if( !SymbolTables.TrySearchUITypeByName( modifier, out var uiType ) )
            {
                EventEmitter.Emit(
                    node.AsErrorEvent(
                        CompilerMessageResources.semantic_error_declare_variable_unkown_ui,
                        modifier
                    )
                );

                return;
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
    }

    #endregion ~Primary validations

    #region Initializer root

    private void ValidateInitialValue( IAstVisitor visitor, AstVariableDeclarationNode node, VariableSymbol variable )
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

            return;
        }

        // constなし＋初期化代入式がない場合はスキップ
        if( node.Initializer.IsNull() )
        {
            // 例外として文字列型は宣言時に初期化代入はできないので宣言 = 初期化済み としてマーク
            if( variable.DataType.IsString() )
            {
                variable.State = SymbolState.Initialized;
            }

            return;
        }

        ValidateVariableInitializer( visitor, node, variable );

        // 以降の解析を継続させるため、初期化式に誤りがあっても初期化済みとしてマーク
        variable.State = SymbolState.Initialized;
    }

    private void ValidateVariableInitializer( IAstVisitor visitor, AstVariableDeclarationNode node, VariableSymbol variable )
    {
        if( variable.Modifier.IsUI() )
        {
            ValidateUIInitializer( visitor, node, variable );

            return;
        }
        else if( variable.DataType.IsArray() )
        {
            ValidateArrayInitializer( visitor, node, variable );

            return;
        }

        ValidatePrimitiveInitializer( visitor, node, variable );
    }

    #endregion ~Initializer root

    #region Primitive Initializer

    private void ValidatePrimitiveInitializer( IAstVisitor visitor, AstVariableDeclarationNode node, VariableSymbol variable )
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

            return;
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

            return;
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

            return;
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

            return;
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

            return;
        }

        // 代入値が畳み込みされた値（リテラル値）であれば、右項をその値に置き換える
        if( evaluated.IsLiteralNode() )
        {
            initializer.Expression = evaluated;
        }

        // 定数型変数に対して初期化式がリテラル値であれば、その値を保持
        if( variable.Modifier.IsConstant() && evaluated.TryGetLiteralNodeValue( out var value ))
        {
            variable.ConstantValue = value;
        }
    }

    #endregion ~Primitive Initializer

    #region Array Initializer

    private void ValidateArrayInitializer( IAstVisitor visitor, AstVariableDeclarationNode node, VariableSymbol variable )
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

            return;
        }

        if( !ValidateArraySize( visitor, node, initializer, variable ) )
        {
            return;
        }

        // 初期化代入式なし
        if( initializer.IsNull() )
        {
            EventEmitter.Emit(
                node.AsErrorEvent(
                    CompilerMessageResources.semantic_error_declare_variable_required_initializer,
                    node.Name
                )
            );

            return;
        }

        // 文法解析で判定したかったが、KSPの文法仕様（配列変数とUI配列変数の初期値代入）が
        // 微妙な差があるため、意味解析でチェック（ := の有無の違い、という微妙な差を文法解析で表現すると複雑になる）
        if( !initializer.Initializer.Expressions.Empty && !initializer.HasAssignOperator )
        {
            EventEmitter.Emit(
                node.AsErrorEvent(
                    CompilerMessageResources.semantic_error_declare_variable_arrayinitializer_need_assign_operator,
                    node.Name
                )
            );

            return;
        }

        // 配列要素の型チェック
        ValidateArrayElements( visitor, node, variable, initializer );
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

    private void ValidateArrayElements( IAstVisitor visitor, AstVariableDeclarationNode node, VariableSymbol variable, AstArrayInitializerNode initializer )
    {
        // 初期値代入なし
        if( initializer.Initializer.IsNull() )
        {
            return;
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

                continue;
            }

            // 型の一致チェック
            if( !TypeCompatibility.IsAssigningTypeCompatible( variable.DataType.TypeMasked(), evaluated.TypeFlag ) )
            {
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
    }

    #endregion ~Array Initializer

    #region UI Initializer

    private void ValidateUIInitializer( IAstVisitor visitor, AstVariableDeclarationNode node, VariableSymbol variable )
    {
        if( variable.DataType.IsArray() )
        {
            ValidateArrayBasedUIInitializer( visitor, node, node.Initializer.ArrayInitializer, variable );

            return;
        }

        ValidatePrimitiveBasedUIInitializer( visitor, node, node.Initializer.PrimitiveInitializer.UIInitializer, variable );
    }

    private void ValidateArrayBasedUIInitializer( IAstVisitor visitor, AstVariableDeclarationNode node, AstArrayInitializerNode initializer, VariableSymbol variable )
    {
        if( !ValidateArraySize( visitor, node, initializer, variable ) )
        {
            return;
        }

        // 文法解析で判定したかったが、KSPの文法仕様（配列変数とUI配列変数の初期値代入）が
        // 微妙な差があるため、意味解析でチェック（ := の有無の違い、という微妙な差を文法解析で表現すると複雑になる）
        if( initializer.HasAssignOperator )
        {
            EventEmitter.Emit(
                node.AsErrorEvent(
                    CompilerMessageResources.semantic_error_declare_variable_uiinitializer_not_need_assign_opetator,
                    node.Name
                )
            );

            return;
        }

        // 配列要素の型チェック
        ValidateUIArguments( visitor, node, initializer.Initializer, variable.UIType );
    }

    private void ValidatePrimitiveBasedUIInitializer( IAstVisitor visitor, AstVariableDeclarationNode node, AstExpressionListNode initializer, VariableSymbol variable )
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

            return;
        }

        ValidateUIArguments( visitor, node, initializer, variable.UIType );
    }

    private void ValidateUIArguments( IAstVisitor visitor, AstVariableDeclarationNode node, AstExpressionListNode expressionList, UITypeSymbol uiType )
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

            return;
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

                return;
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

                return;
            }

            // 引数が畳み込みでリテラルになっていれば引数の式を置き換える
            if( evaluated.IsLiteralNode() )
            {
                expressionList.Expressions[ i ] = evaluated;
            }
        }
    }

    #endregion ~UI Initializer

}
