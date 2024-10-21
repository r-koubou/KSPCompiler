using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Declarations;
using KSPCompiler.Domain.Ast.Extensions;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
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

    public IAstNode Evaluate( AstVariableDeclarationNode node )
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

        //
        if( !ValidateUIType( node, variable ) )
        {
            return node;
        }

        if( !VariableSymbols.Add( variable ) )
        {
            throw new AstAnalyzeException( this, node, $"Variable symbol add failed {variable.Name}" );
        }

        return node;
    }

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
        if( !variable.DataTypeModifier.IsUI() )
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
        if( uiType.DataTypeModifier.IsConstant() )
        {
            variable.DataTypeModifier |= DataTypeModifierFlag.Const;
        }

        // UI型情報を参照
        // 意味解析時に使用するため、用意されている変数に保持
        variable.UIType = uiType;

        return true;
    }
}
