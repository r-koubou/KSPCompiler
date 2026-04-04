using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Completion;
using KSPCompiler.Features.LanguageServer.UseCase.Completion.CompletionItems;
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
#warning リリース前にテストコードを消す
            var preferSnippetInsertion = true;
            //var preferSnippetInsertion = parameter.Input.PreferSnippetInsertion;

            var cache = compilerCacheService.GetCache( scriptLocation );
            var symbolTable = cache.SymbolTable;
            var word = DocumentUtility.ExtractWord( cache.AllLinesText, position );
            var completions = new List<CompletionItem>();

            #region Collection of target symbols
            // プリプロセッサ
            var preprocessors = MatchCompletionItem(
                symbolTable.PreProcessorSymbols,
                word
            );

            // PGS KeyId
            var pgsKeyIds = MatchCompletionItem(
                symbolTable.PgsKeyIdSymbolTable,
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
            var snippetTextBuilder = new StringBuilder();
            var preprocessorItemFactory = new PreprocessorCompletionItemFactory();
            var pgsItemFactory = new PgsKeyCompletionItemFactory();
            var variableItemFactory = new VariableCompletionItemFactory();
            var commandItemFactory = new CommandCompletionItemFactory( snippetTextBuilder );
            var callbackItemFactory = new CallbackCompletionItemFactory( snippetTextBuilder );
            var userFunctionItemFactory = new UserFunctionCompletionItemFactory( snippetTextBuilder );

            BuildCompletionItemNew( preprocessors, preprocessorItemFactory, word, preferSnippetInsertion, completions );
            BuildCompletionItemNew( pgsKeyIds, pgsItemFactory, word, preferSnippetInsertion, completions );
            BuildCompletionItemNew( userVariables, variableItemFactory, word, preferSnippetInsertion, completions );
            BuildCompletionItemNew( builtInVariables, variableItemFactory, word, preferSnippetInsertion, completions );
            //BuildCompletionItemNew( uiTypes, word, CompletionItemKind.Class, preferSnippetInsertion, "UI Type", completions );
            BuildCompletionItemNew( commands, commandItemFactory, word, preferSnippetInsertion, completions );
            BuildCompletionItemNew( userFunctions, userFunctionItemFactory, word, preferSnippetInsertion, completions );
            BuildCompletionItemNew( builtInCallBacks, callbackItemFactory, word, preferSnippetInsertion, completions );

            // BuildCompletionItem( preprocessors, word, CompletionItemKind.Variable, preferSnippetInsertion, "Preprocessor", completions );
            // BuildCompletionItem( pgsKeyIds, word, CompletionItemKind.Variable, preferSnippetInsertion, "PGS Key-Id", completions );
            // BuildCompletionItem( userVariables, word, CompletionItemKind.Variable, preferSnippetInsertion, "User Variable", completions );
            // BuildCompletionItem( builtInVariables, word, CompletionItemKind.Function, preferSnippetInsertion, "Built-in Variable", completions );
            // BuildCompletionItem( uiTypes, word, CompletionItemKind.Class, preferSnippetInsertion, "UI Type", completions );
            // BuildCompletionItem( commands, word, CompletionItemKind.Method, preferSnippetInsertion, "Command", completions );
            // BuildCompletionItem( userFunctions, word, CompletionItemKind.Function, preferSnippetInsertion, "User Function", completions );
            // BuildCompletionItem( builtInCallBacks, word, CompletionItemKind.Event, preferSnippetInsertion, "Callback", completions );
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

    private static void BuildCompletionItemNew<TSymbol>(
        IReadOnlyCollection<TSymbol> symbols,
        ICompletionItemFactory<TSymbol> itemFactory,
        string partialName,
        bool preferSnippetInsertion,
        List<CompletionItem> target ) where TSymbol : SymbolBase
    {
        if( itemFactory.TryCreateFixedSnippet( partialName, out var fixedSnippetItem ) )
        {
            target.Add( fixedSnippetItem );
        }

        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach( var symbol in symbols )
        {
            var completionItem = itemFactory.Create( symbol, partialName, preferSnippetInsertion );
            target.Add( completionItem );
        }
    }

#warning リリース前に旧コードを消す
    #region Will be removed code
    [Obsolete( "Use BuildCompletionItemNew with ICompletionItemFactory instead." )]
    private static void BuildCompletionItem<TSymbol>(
        IReadOnlyCollection<TSymbol> symbols,
        string partialName,
        CompletionItemKind kind,
        bool preferSnippetInsertion,
        string detail,
        List<CompletionItem> target ) where TSymbol : SymbolBase
    {
        var stringBuilder = new StringBuilder( 256 );

        var preprocessorItemFactory = new PreprocessorCompletionItemFactory();
        var pgsItemFactory = new PgsKeyCompletionItemFactory();
        var variableItemFactory = new VariableCompletionItemFactory();
        var commandItemFactory = new CommandCompletionItemFactory( stringBuilder );
        var callbackItemFactory = new CallbackCompletionItemFactory( stringBuilder );
        var userFunctionItemFactory = new UserFunctionCompletionItemFactory( stringBuilder );

        foreach( var symbol in symbols )
        {
            stringBuilder.Clear();

            switch( symbol )
            {
                case CallbackSymbol callbackSymbol:
                {
                    // preferSnippetInsertion: fixed to true
                    // Always expand the snippet if the phrase starts with “on”
                    var completionItem = callbackItemFactory.Create( callbackSymbol, partialName, true );
                    target.Add( completionItem );

                    break;
                }

                case CommandSymbol commandSymbol:
                {
                    var completionItem = commandItemFactory.Create( commandSymbol, partialName, preferSnippetInsertion );
                    target.Add( completionItem );

                    continue;
                }

                case VariableSymbol variableSymbol:
                {
                    var item = variableItemFactory.Create( variableSymbol, partialName, preferSnippetInsertion );

                    target.Add( item );

                    break;
                }

                case UserFunctionSymbol userFunctionSymbol:
                {
                    var item = userFunctionItemFactory.Create( userFunctionSymbol, partialName, preferSnippetInsertion );

                    target.Add( item );

                    break;
                }

                case PreProcessorSymbol preProcessorSymbol:
                {
                    var item = preprocessorItemFactory.Create( preProcessorSymbol, partialName, preferSnippetInsertion );

                    target.Add( item );

                    break;
                }

                case PgsSymbol pgsSymbol:
                {
                    var item = pgsItemFactory.Create( pgsSymbol, partialName, preferSnippetInsertion );

                    target.Add( item );

                    break;
                }

                default:
                {
                    var document = DocumentUtility.GetCommentOrDescriptionText( symbol );
                    document = string.IsNullOrEmpty( document ) ? null : document;

                    target.Add(
                        new CompletionItem(
                            Label: symbol.Name.Value,
                            Kind: kind,
                            Detail: detail,
                            Documentation: document,
                            InsertTextFormat: preferSnippetInsertion ? InsertTextFormat.Snippet : InsertTextFormat.PlainText,
                            InsertText: symbol.Name.Value
                        )
                    );
                    break;
                }
            }

        }
    }

    [Obsolete( "Use CallbackCompletionItemFactory instead." )]
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

    [Obsolete( "Use VariableCompletionItemFactory instead." )]
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

    [Obsolete( "Use CommandCompletionItemFactory instead." )]
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
    #endregion
}
