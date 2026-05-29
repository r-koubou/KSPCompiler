using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Hover;
using KSPCompiler.Features.LanguageServer.UseCase.Hover.Extensions;
using KSPCompiler.Shared;
using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.LanguageServer.UseCase.Hover;

public sealed class HoverInteractor : IHoverUseCase
{
    public async Task<Result<HoverOutput, LanguageServerFailureReason>> ExecuteAsync(
        HoverInput input,
        CancellationToken cancellationToken = default )
    {
        try
        {
            var compilationCacheManager = input.Cache;
            var scriptLocation = input.Location;
            var position = input.Position;

            var hoverItemNotFoundResult = Result<HoverOutput, LanguageServerFailureReason>.Failure( LanguageServerFailureReason.HoverItemNotFound );

            if( !compilationCacheManager.ContainsCache( scriptLocation ) )
            {
                return hoverItemNotFoundResult;
            }

            var cache = compilationCacheManager.GetCache( scriptLocation );
            var symbols = cache.SymbolTable;
            var word = DocumentUtility.ExtractWord( cache.AllLinesText, position );

            if( string.IsNullOrEmpty( word ) )
            {
                return hoverItemNotFoundResult;
            }

            #region User deffinitions
            // ユーザー定義変数(コメントがある場合)
            if( symbols.UserVariables.TryBuildHoverText( word, out var hoverText, new UserDefinedSymbolHoverTextBuilder<VariableSymbol>() ) )
            {
                return Result<HoverOutput, LanguageServerFailureReason>.Success( new HoverOutput( hoverText.AsHover() ) );
            }

            // ユーザー定義関数(コメントがある場合)
            if( symbols.UserFunctions.TryBuildHoverText( word, out hoverText, new UserDefinedSymbolHoverTextBuilder<UserFunctionSymbol>() ) )
            {
                return Result<HoverOutput, LanguageServerFailureReason>.Success( new HoverOutput( hoverText.AsHover() ) );
            }

            // TODO
            // // ユーザー定義コールバック(コメントがある場合)
            // if( symbols.UserCallbacks.TryBuildHoverText(word, out hoverText, new UserDefinedSymbolHoverTextBuilder<CallbackSymbol>() ) )
            // {
            //      return Result<HoverOutput, LanguageServerFailureReason>.Success( new HoverOutput( hoverText.AsHover() ) );
            // }
            #endregion ~User deffinitions

            #region BuiltIn
            // ビルトイン変数
            if( symbols.BuiltInVariables.TryBuildHoverText( word, out hoverText, new VariableHoverTextBuilder() ) )
            {
                return Result<HoverOutput, LanguageServerFailureReason>.Success( new HoverOutput( hoverText.AsHover() ) );
            }

            // UI型
            if( symbols.UITypes.TryBuildHoverText( word, out hoverText, new UITypeHoverTextBuilder() ) )
            {
                return Result<HoverOutput, LanguageServerFailureReason>.Success( new HoverOutput( hoverText.AsHover() ) );
            }

            // コマンド
            if( symbols.Commands.TryBuildHoverText( word, out hoverText, new CommandHoverTextBuilder() ) )
            {
                return Result<HoverOutput, LanguageServerFailureReason>.Success( new HoverOutput( hoverText.AsHover() ) );
            }
            #endregion ~BuiltIn

            await Task.CompletedTask;

            return hoverItemNotFoundResult;
        }
        catch( Exception e )
        {
            return Result<HoverOutput, LanguageServerFailureReason>.Failure( LanguageServerFailureReason.Other, e );
        }
    }
}
