using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Applications.LSPServer.CoreNew.Compilation;
using KSPCompiler.Applications.LSPServer.CoreNew.Hover.Extensions;
using KSPCompiler.Commons.Text;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Applications.LSPServer.CoreNew.Hover;

public sealed class HoverHandlingService
{
    public async Task<HoverItem?> HandleAsync(
        CompilationCacheManager compilerCacheManager,
        ScriptLocation scriptLocation,
        Position position,
        CancellationToken _ )
    {
        if( !compilerCacheManager.ContainsCache( scriptLocation ) )
        {
            return null;
        }

        var cache = compilerCacheManager.GetCache( scriptLocation );
        var symbols = cache.SymbolTable;
        var word = DocumentUtility.ExtractWord( cache.AllLinesText, position );

        if( string.IsNullOrEmpty( word ) )
        {
            return null;
        }

        #region User deffinitions
        // ユーザー定義変数(コメントがある場合)
        if( symbols.UserVariables.TryBuildHoverText(word, out var hoverText, new UserDefinedSymbolHoverTextBuilder<VariableSymbol>() ) )
        {
            return hoverText.AsHover();
        }

        // ユーザー定義関数(コメントがある場合)
        if( symbols.UserFunctions.TryBuildHoverText(word, out hoverText, new UserDefinedSymbolHoverTextBuilder<UserFunctionSymbol>() ) )
        {
            return hoverText.AsHover();
        }

        // TODO
        // // ユーザー定義コールバック(コメントがある場合)
        // if( symbols.UserCallbacks.TryBuildHoverText(word, out hoverText, new UserDefinedSymbolHoverTextBuilder<CallbackSymbol>() ) )
        // {
        //     return hoverText.AsHover();
        // }

        #endregion ~User deffinitions

        #region BuiltIn
        // ビルトイン変数
        if( symbols.BuiltInVariables.TryBuildHoverText( word, out hoverText, new VariableHoverTextBuilder() ) )
        {
            return hoverText.AsHover();
        }

        // UI型
        if( symbols.UITypes.TryBuildHoverText( word, out hoverText, new UITypeHoverTextBuilder() ) )
        {
            return hoverText.AsHover();
        }

        // コマンド
        if( symbols.Commands.TryBuildHoverText( word, out hoverText, new CommandHoverTextBuilder() ) )
        {
            return hoverText.AsHover();
        }

        #endregion ~BuiltIn

        await Task.CompletedTask;

        return null;
    }
}
