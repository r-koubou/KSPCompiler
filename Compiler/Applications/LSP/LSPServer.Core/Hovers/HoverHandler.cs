using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.LSPServer.Core.Compilations;

using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.LSPServer.Core.Hovers;

public class HoverHandler( HoverService hoverService ) : IHoverHandler
{
    private HoverService HoverService { get; } = hoverService;

    public async Task<Hover?> Handle( HoverParams request, CancellationToken cancellationToken )
        => await HoverService.HandleAsync( request, cancellationToken );

    public HoverRegistrationOptions GetRegistrationOptions( HoverCapability capability, ClientCapabilities clientCapabilities )
        => new()
        {
            DocumentSelector = ConstantValues.TextDocumentSelector
        };
}

public sealed class HoverService( CompilerCacheService compilerCacheService )
{
    private CompilerCacheService CompilerCacheService { get; } = compilerCacheService;

    public async Task<Hover?> HandleAsync( HoverParams request, CancellationToken cancellationToken )
    {
        var cache = CompilerCacheService.GetCache( request.TextDocument.Uri );
        var word = DocumentUtility.ExtractWord( cache.AllLinesText, request.Position );

        #region User deffinitions
        // ユーザー定義変数(コメントがある場合)
        if( TryBuildHoverForUserDefinition( cache.SymbolTable.UserVariables, word, out var hover ) )
        {
            return hover;
        }

        // ユーザー定義関数(コメントがある場合)
        if( TryBuildHoverForUserDefinition( cache.SymbolTable.UserFunctions, word, out hover ) )
        {
            return hover;
        }

        #endregion ~User deffinitions

        #region BuiltIn
        // ビルトイン変数
        if( TryBuildHover( cache.SymbolTable.BuiltInVariables, word, out hover ) )
        {
            return hover;
        }

        // UI型
        if( TryBuildHover( cache.SymbolTable.UITypes, word, out hover, GetUIypeHoverText ) )
        {
            return hover;
        }

        // コマンド
        if( TryBuildHover( cache.SymbolTable.Commands, word, out hover, GetCommandHoverText ) )
        {
            return hover;
        }
        #endregion ~BuiltIn

        await Task.CompletedTask;

        return null;
    }

    private static bool TryBuildHover<TSymbol>(
        ISymbolTable<TSymbol> symbols,
        string word,
        out Hover? result,
        Func<TSymbol, string>? hoverTextBuilder = null )
        where TSymbol : SymbolBase
    {
        result = null;

        if( !symbols.TrySearchByName( word, out var symbol ) )
        {
            return false;
        }

        var hoverText = symbol.Description.Value;

        if( hoverTextBuilder!= null )
        {
            hoverText = hoverTextBuilder( symbol );
        }

        if( string.IsNullOrWhiteSpace( hoverText ) )
        {
            return false;
        }

        result = new Hover
        {
            Contents = new MarkedStringsOrMarkupContent(
                new MarkedString( hoverText )
            )
        };

        return true;
    }

    private static bool TryBuildHoverForUserDefinition<TSymbol>(
        ISymbolTable<TSymbol> symbols,
        string word,
        out Hover? result )
        where TSymbol : SymbolBase
    {
        return TryBuildHover(
            symbols,
            word,
            out result,
            symbol => BuildHoverTextFromUserComment( symbol.CommentLines )
        );
    }

    private static string GetUIypeHoverText( UITypeSymbol uiTypeSymbol )
    {
        var builder = new StringBuilder( 64 );

        builder.AppendLine( $"**{uiTypeSymbol.Name}**" );
        builder.AppendLine();
        builder.AppendLine( uiTypeSymbol.Description.Value );

        return builder.ToString();
    }

    private static string GetCommandHoverText( CommandSymbol commandSymbol )
    {
        var builder = new StringBuilder( 64 );

        /*
         * command_name(arg1, arg2, ...)
         * Description
         */

        builder.Append( $"**{commandSymbol.Name}" );

        if( commandSymbol.ArgumentCount > 0 )
        {
            var index = 0;
            var argCount = commandSymbol.Arguments.Count;

            builder.Append( "(" );

            foreach( var arg in commandSymbol.Arguments )
            {
                builder.Append( $"{arg.Name}" );

                if( index < argCount - 1 )
                {
                    builder.Append( ", " );
                }

                index++;
            }

            builder.Append( ")" );
            builder.AppendLine( "**" );
        }
        else
        {
            builder.AppendLine( "**" );
            builder.AppendLine();
        }

        builder.AppendLine();
        builder.AppendLine( commandSymbol.Description.Value );

        if( commandSymbol.ArgumentCount == 0 )
        {
            return builder.ToString();
        }

        builder.AppendLine();

        /*
         * Arguments:
         *   - arg1
         *     - type1
         *  - arg2
         *    - type2
         * :
         * :
         */
        builder.AppendLine( "**Arguments:**" );

        foreach( var arg in commandSymbol.Arguments )
        {
            var argType = arg.DataType.ToMessageString();

            builder.AppendLine( $"- {arg.Name}" );

            if( arg.UITypeNames.Count > 0 )
            {
                builder.AppendLine( $"  - {string.Join( ", ", arg.UITypeNames )}" );
            }
            else
            {
                builder.AppendLine( $"  - {argType}" );
            }
        }

        return builder.ToString();
    }

    private static string BuildHoverTextFromUserComment( IReadOnlyCollection<string> commentLines )
    {
        var builder = new StringBuilder( 64 );

        var  minIndent = commentLines
                       .Where( line => line.Trim().Length > 0 )
                       .Select( line => line.Length - line.TrimStart().Length )
                       .DefaultIfEmpty( 0 )
                       .Min();

        var normalizedLines = commentLines
           .Select( line => line.Length >= minIndent ? line.Substring( minIndent ) : line );

        foreach( var line in normalizedLines )
        {
            builder.AppendLine( line );
        }

        return builder.ToString();
    }
}
