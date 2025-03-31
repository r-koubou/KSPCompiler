using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Completion;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData.Extensions;

namespace KSPCompiler.Features.LanguageServer.UseCase.Completion;

public sealed class CompletionInteractor : ICompletionUseCase
{
    public async Task<CompletionHandlingOutput> ExecuteAsync( CompletionHandlingInputPort parameter, CancellationToken cancellationToken = default )
    {
        try
        {
            var compilerCacheService = parameter.Input.Cache;
            var scriptLocation = parameter.Input.Location;
            var position = parameter.Input.Position;

            var cache = compilerCacheService.GetCache( scriptLocation );
            var symbolTable = cache.SymbolTable;
            var line = cache.AllLinesText[ position.BeginLine.Value ];
            var word = DocumentUtility.ExtractWord( cache.AllLinesText, position );
            var completions = new List<CompletionItem>();

            #region Collection of target symbols
            // プリプロセッサ
            var preprocessors = MatchCompletionItem(
                symbolTable.PreProcessorSymbols,
                word
            );

            // ユーザー定義変数
            var userVariables = MatchCompletionItem(
                symbolTable.UserVariables,
                word
            );

            // ビルトイン変数
            var builtInVariables = MatchCompletionItem(
                symbolTable.BuiltInVariables,
                word
            );

            // UI型
            var uiTypes = MatchCompletionItem(
                symbolTable.UITypes,
                word
            );

            // コマンド
            var commands = MatchCompletionItem(
                symbolTable.Commands,
                word
            );

            // ユーザー定義関数
            var userFunctions = MatchCompletionItem(
                symbolTable.UserFunctions,
                word
            );

            // ビルトインコールバック
            // onをトリガーにすべてのビルトインコールバックを表示する
            // ユーザー定義のコールバックについてははビルトインのコールバック名なので扱わない
            var builtInCallBacks = symbolTable.BuiltInCallbacks.ToList();
            #endregion ~Collection of target symbols

            #region Build completion list
            BuildCompletionItem( preprocessors, word, CompletionItemKind.Variable, "Preprocessor", completions );
            BuildCompletionItem( userVariables, word, CompletionItemKind.Variable, "User Variable", completions );
            BuildCompletionItem( builtInVariables, word, CompletionItemKind.Function, "Built-in Variable", completions );
            BuildCompletionItem( uiTypes, word, CompletionItemKind.Class, "UI Type", completions );
            BuildCompletionItem( commands, word, CompletionItemKind.Method, "Command", completions );
            BuildCompletionItem( userFunctions, word, CompletionItemKind.Function, "User Function", completions );
            BuildCompletionItem( builtInCallBacks, word, CompletionItemKind.Event, "Callback", completions );
            #endregion ~Build completion list

            await Task.CompletedTask;

            return new CompletionHandlingOutput( completions, true );
        }
        catch( Exception e )
        {
            return new CompletionHandlingOutput( [], false, e );
        }
    }

    private static List<TSymbol> MatchCompletionItem<TSymbol>(
        ISymbolTable<TSymbol> symbols,
        string partialName,
        Func<TSymbol, bool>? extracCondition = null )
        where TSymbol : SymbolBase
    {
        return symbols.Where(
            x => x.Name.Value != partialName
                 && x.Name.Value.Contains( partialName )
                 && ( extracCondition == null || extracCondition.Invoke( x ) )
        ).ToList();
    }

    private static List<TSymbol> MatchCompletionItem<TSymbol, TOverload>(
        IOverloadedSymbolTable<TSymbol, TOverload> symbols,
        string partialName,
        Func<TSymbol, bool>? extracCondition = null )
        where TSymbol : SymbolBase
        where TOverload : IEquatable<TOverload>
    {
        var list = symbols.Where(
            x => x.First().Value.Name != partialName
                 && x.First().Value.Name.Value.Contains( partialName )
                 && ( extracCondition == null || extracCondition.Invoke( x.First().Value ) )
        ).ToList();

        return list.Select( x => x.First().Value ).ToList();
    }

    private static void BuildCompletionItem<TSymbol>(
        IReadOnlyCollection<TSymbol> symbols,
        string partialName,
        CompletionItemKind kind,
        string detail,
        List<CompletionItem> target ) where TSymbol : SymbolBase
    {
        var stringBuilder = new StringBuilder( 256 );

        foreach( var symbol in symbols )
        {
            stringBuilder.Clear();

            if( symbol is CallbackSymbol callbackSymbol )
            {
                if( TryBuildCallbackSnippetItem( callbackSymbol, partialName, stringBuilder, out var completionItem ) )
                {
                    target.Add( completionItem );

                    continue;
                }
            }

            if( symbol is CommandSymbol commandSymbol )
            {
                var completionItem = BuildCommandItem( commandSymbol );

                target.Add( completionItem );

                continue;
            }

            if( symbol is VariableSymbol variableSymbol )
            {
                var item = BuildVariableItem( variableSymbol, partialName );

                target.Add( item );

                continue;
            }

            var document = DocumentUtility.GetCommentOrDescriptionText( symbol );
            document = string.IsNullOrEmpty( document ) ? null : document;

            target.Add(
                new CompletionItem(
                    Label: symbol.Name.Value,
                    Kind: kind,
                    Detail: detail,
                    Documentation: document,
                    InsertTextFormat: InsertTextFormat.PlainText,
                    InsertText: symbol.Name.Value
                )
            );
        }
    }

    private static bool TryBuildCallbackSnippetItem( CallbackSymbol callbackSymbol, string partialName, StringBuilder stringBuilder, out CompletionItem result )
    {
        var document = DocumentUtility.GetCommentOrDescriptionText( callbackSymbol );
        document = string.IsNullOrEmpty( document ) ? null : document;

        result = null!;

        if( !partialName.StartsWith( "on" ) )
        {
            return false;
        }

        if( callbackSymbol.ArgumentCount > 0 )
        {
            stringBuilder.Append( "on " ).Append( callbackSymbol.Name.Value ).Append( '(' );

            for( var snippetIndex = 0; snippetIndex < callbackSymbol.ArgumentCount; snippetIndex++ )
            {
                stringBuilder.Append( "${" )
                             .Append( snippetIndex + 1 ).Append( ':' )
                             .Append( callbackSymbol.Arguments[ snippetIndex ].Name.Value )
                             .Append( '}' );

                if( snippetIndex < callbackSymbol.ArgumentCount - 1 )
                {
                    stringBuilder.Append( ", " );
                }
            }

            var codeIndex = callbackSymbol.ArgumentCount + 1;

            stringBuilder.AppendLine( ")" )
                         .AppendLine( $"    ${{{codeIndex}:code}}" )
                         .AppendLine( "end on" );
        }
        else
        {
            stringBuilder.Append( "on " ).AppendLine( callbackSymbol.Name.Value )
                         .AppendLine( "    ${1:code}" )
                         .AppendLine( "end on" );
        }

        result = new CompletionItem(
            Label: $"on {callbackSymbol.Name.Value}",
            Kind: CompletionItemKind.Snippet,
            Detail: "Callback",
            Documentation: document,
            InsertTextFormat: InsertTextFormat.Snippet,
            InsertText: stringBuilder.ToString()
        );

        return true;
    }

    private static CompletionItem BuildVariableItem( VariableSymbol variableSymbol, string partialName )
    {
        var insertText = variableSymbol.Name.Value;
        var kind = CompletionItemKind.Variable;

        var document = DocumentUtility.GetCommentOrDescriptionText( variableSymbol );
        document = string.IsNullOrEmpty( document ) ? null : document;

        if( !variableSymbol.BuiltIn && variableSymbol.Modifier.IsConstant() )
        {
            kind = CompletionItemKind.Constant;
        }

        return new CompletionItem(
            Label: variableSymbol.Name.Value,
            Kind: kind,
            Detail: variableSymbol.BuiltIn ? "Built-in Variable" : "User Variable",
            Documentation: document,
            InsertTextFormat: InsertTextFormat.PlainText,
            InsertText: insertText
        );
    }

    private static CompletionItem BuildCommandItem( CommandSymbol commandSymbol )
    {
        var document = DocumentUtility.GetCommentOrDescriptionText( commandSymbol );
        document = string.IsNullOrEmpty( document ) ? null : document;

        return new CompletionItem(
            Label: commandSymbol.Name.Value,
            Kind: CompletionItemKind.Method,
            Detail: "Command",
            InsertTextFormat: InsertTextFormat.PlainText,
            Documentation: document,
            InsertText: commandSymbol.Name.Value
        );
    }

    private static CompletionItem BuildCommandSnippet( CommandSymbol commandSymbol, StringBuilder stringBuilder )
    {
        var document = DocumentUtility.GetCommentOrDescriptionText( commandSymbol );
        document = string.IsNullOrEmpty( document ) ? null : document;

        if( commandSymbol.Arguments.Count == 0 )
        {
            return new CompletionItem(
                Label: commandSymbol.Name.Value,
                Kind: CompletionItemKind.Method,
                Detail: "Command",
                Documentation: document,
                InsertTextFormat: InsertTextFormat.PlainText,
                InsertText: commandSymbol.Name.Value
            );
        }

        stringBuilder.Append( commandSymbol.Name.Value );
        stringBuilder.Append( "(" );

        var index = 1;
        var argCount = commandSymbol.Arguments.Count;

        foreach( var arg in commandSymbol.Arguments )
        {
            stringBuilder.Append( "${" )
                         .Append( index ).Append( ":" )
                         .Append( arg.Name.Value )
                         .Append( "}" );

            if( index < argCount )
            {
                stringBuilder.Append( ", " );
            }

            index++;
        }

        stringBuilder.Append( ")" );

        return new CompletionItem(
            Label: commandSymbol.Name.Value,
            Kind: CompletionItemKind.Method,
            Detail: "Command",
            Documentation: document,
            InsertTextFormat: InsertTextFormat.Snippet,
            InsertText: stringBuilder.ToString()
        );
    }
}
