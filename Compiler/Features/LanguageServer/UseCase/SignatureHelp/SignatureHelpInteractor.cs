using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.SignatureHelp;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.SignatureHelp.Extensions;
using KSPCompiler.Shared;

namespace KSPCompiler.Features.LanguageServer.UseCase.SignatureHelp;

public sealed class SignatureHelpInteractor : ISignatureHelpUseCase
{
    public async Task<Result<SignatureHelpOutput, LanguageServerFailureReason>> ExecuteAsync( SignatureHelpInput input, CancellationToken cancellationToken = default )
    {
        try
        {
            var compilationCacheManager = input.Cache;
            var scriptLocation = input.Location;
            var position = input.Position;

            var cache = compilationCacheManager.GetCache( scriptLocation );
            var symbols = cache.SymbolTable;

            var signatureNotFoundResult = Result<SignatureHelpOutput, LanguageServerFailureReason>.Failure( LanguageServerFailureReason.SignatureNotFound );

            if( DocumentUtility.IsInCommentToLeft( cache.AllLinesText, position ) )
            {
                return signatureNotFoundResult;
            }

            var iterator = new BackwardIterator(
                cache.AllLinesText,
                position.BeginLine.Value - 1,     // 0-based
                position.BeginColumn.Value - 1 // 0-based and caret position - 1
            );

            var activeParameter = GetActiveArgument( iterator );

            if( activeParameter < 0 )
            {
                return signatureNotFoundResult;
            }

            var word = GetIdentifier( iterator );

            if( string.IsNullOrEmpty( word ) )
            {
                return signatureNotFoundResult;
            }

            if( !symbols.Commands.TryBuildSignatureHelp( word, activeParameter, out var signatureHelp ) )
            {
                return signatureNotFoundResult;
            }

            await Task.CompletedTask;

            return Result<SignatureHelpOutput, LanguageServerFailureReason>.Success( new SignatureHelpOutput( signatureHelp ) );
        }
        catch( Exception e )
        {
            return Result<SignatureHelpOutput, LanguageServerFailureReason>.Failure( LanguageServerFailureReason.Other, e );
        }
    }

    //--------------------------------------------------------------------------------------------------------
    // Implemented based on Part of PHP Signature Help Provider implementation. (signatureHelpProvider.ts)
    // https://github.com/microsoft/vscode/blob/main/extensions/php-language-features/src/features/signatureHelpProvider.ts
    //--------------------------------------------------------------------------------------------------------

    private static int GetActiveArgument( BackwardIterator iterator )
    {
        var parentNestDepth = 0;
        var bracketNestDepth = 0;
        var activeArgument = 0;

        while( iterator.HasNext )
        {
            var c = iterator.GetNext();

            switch( c )
            {
                case '(':
                    parentNestDepth--;

                    if( parentNestDepth < 0 )
                    {
                        return activeArgument;
                    }

                    break;
                case ')':
                    parentNestDepth++;

                    break;

                case '[':
                    bracketNestDepth--;

                    break;

                case ']':
                    bracketNestDepth++;

                    break;

                case ',':
                    if( parentNestDepth == 0 && bracketNestDepth == 0 )
                    {
                        activeArgument++;
                    }
                    break;
            }
        }

        return -1;
    }

    //--------------------------------------------------------------------------------------------------------
    // Implemented based on Part of PHP Signature Help Provider implementation. (signatureHelpProvider.ts)
    // https://github.com/microsoft/vscode/blob/main/extensions/php-language-features/src/features/signatureHelpProvider.ts
    //--------------------------------------------------------------------------------------------------------

    private static string GetIdentifier( BackwardIterator iterator )
    {
        var identStarted = false;
        var ident = string.Empty;

        while( iterator.HasNext )
        {
            var ch = iterator.GetNext();

            if( !identStarted && DocumentUtility.IsSkipChar( ch ) )
            {
                continue;
            }

            if( DocumentUtility.IsIdentifierChar( ch ) )
            {
                identStarted = true;
                ident        = ch + ident;
            }
            else if( identStarted )
            {
                return ident;
            }
        }

        return ident;
    }
}
