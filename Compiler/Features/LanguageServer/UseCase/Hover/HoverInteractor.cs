using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Hover;
using KSPCompiler.Features.LanguageServer.UseCase.Hover.Extensions;
using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.LanguageServer.UseCase.Hover;

public sealed class HoverInteractor : IHoverUseCase
{
    public async Task<HoverOutputPort> ExecuteAsync(
        HoverInputPort parameter,
        CancellationToken cancellationToken = default )
    {
        try
        {
            var compilationCacheManager = parameter.Input.Cache;
            var scriptLocation = parameter.Input.Location;
            var position = parameter.Input.Position;

            if( !compilationCacheManager.ContainsCache( scriptLocation ) )
            {
                return new HoverOutputPort( null, true );
            }

            var cache = compilationCacheManager.GetCache( scriptLocation );
            var symbols = cache.SymbolTable;
            var word = DocumentUtility.ExtractWord( cache.AllLinesText, position );

            if( string.IsNullOrEmpty( word ) )
            {
                return new HoverOutputPort( null, true );
            }

            #region User deffinitions
            // ユーザー定義変数(コメントがある場合)
            if( symbols.UserVariables.TryBuildHoverText( word, out var hoverText, new UserDefinedSymbolHoverTextBuilder<VariableSymbol>() ) )
            {
                return new HoverOutputPort( hoverText.AsHover(), true );
            }

            // ユーザー定義関数(コメントがある場合)
            if( symbols.UserFunctions.TryBuildHoverText( word, out hoverText, new UserDefinedSymbolHoverTextBuilder<UserFunctionSymbol>() ) )
            {
                return new HoverOutputPort( hoverText.AsHover(), true );
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
                return new HoverOutputPort( hoverText.AsHover(), true );
            }

            // UI型
            if( symbols.UITypes.TryBuildHoverText( word, out hoverText, new UITypeHoverTextBuilder() ) )
            {
                return new HoverOutputPort( hoverText.AsHover(), true );
            }

            // コマンド
            if( symbols.CommandsNew.TryBuildHoverText( word, out hoverText, new CommandHoverTextBuilderNew() ) )
            {
                return new HoverOutputPort( hoverText.AsHover(), true );
            }
            #endregion ~BuiltIn

            await Task.CompletedTask;

            return new HoverOutputPort( null, true );
        }
        catch( Exception e )
        {
            return new HoverOutputPort( null, false, e );
        }
    }
}
