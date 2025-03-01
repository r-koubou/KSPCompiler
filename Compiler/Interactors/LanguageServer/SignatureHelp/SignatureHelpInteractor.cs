using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.UseCases.LanguageServer.SignatureHelp;
using KSPCompiler.UseCases.LanguageServer.SignatureHelp.Extensions;

namespace KSPCompiler.Interactors.LanguageServer.SignatureHelp;

public sealed class SignatureHelpInteractor : ISignatureHelpUseCase
{
    public async Task<SignatureHelpOutputPort> ExecuteAsync( SignatureHelpInputPort parameter, CancellationToken cancellationToken = default )
    {
        try
        {
            var compilationCacheManager = parameter.Input.Cache;
            var scriptLocation = parameter.Input.Location;
            var position = parameter.Input.Position;

            var cache = compilationCacheManager.GetCache( scriptLocation );
            var symbols = cache.SymbolTable;

            if( DocumentUtility.IsInCommentToLeft( cache.AllLinesText, position ) )
            {
                return new SignatureHelpOutputPort( null, true );
            }

            var iterator = new BackwardIterator(
                cache.AllLinesText,
                position.BeginLine.Value - 1,     // 0-based
                position.BeginColumn.Value - 1 // 0-based and caret position - 1
            );

            var activeParameter = GetActiveArgument( iterator );

            if( activeParameter < 0 )
            {
                return new SignatureHelpOutputPort( null, true );
            }

            var word = GetIdentifier( iterator );

            if( string.IsNullOrEmpty( word ) )
            {
                return new SignatureHelpOutputPort( null, true );
            }

            if( !symbols.Commands.TryBuildSignatureHelp( word, activeParameter, out var signatureHelp ) )
            {
                return new SignatureHelpOutputPort( null, true );
            }

            await Task.CompletedTask;

            return new SignatureHelpOutputPort( signatureHelp, true );
        }
        catch( Exception e )
        {
            return new SignatureHelpOutputPort( null, false, e );
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
