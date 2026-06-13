using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Completion;
using KSPCompiler.Features.LanguageServer.UseCase.Completion.CompletionItems;
using KSPCompiler.Shared;
using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.LanguageServer.UseCase.Completion;

public sealed class CompletionInteractor : ICompletionUseCase
{
    public async Task<Result<CompletionHandlingOutput, LanguageServerFailureReason>> ExecuteAsync( CompletionHandlingInput input, CancellationToken cancellationToken = default )
    {
        try
        {
            var compilerCacheService = input.Cache;
            var scriptLocation = input.Location;
            var position = input.Position;
            var preferSnippetInsertion = input.PreferSnippetInsertion;

            var cache = compilerCacheService.GetCache( scriptLocation );
            var symbolTable = cache.SymbolTable;
            var word = DocumentUtility.ExtractWord( cache.AllLinesText, position );
            var completions = new List<CompletionItem>();

            var snippetTextBuilder = new StringBuilder();

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
            var preprocessorItemFactory = new PreprocessorCompletionItemFactory();
            var pgsItemFactory = new PgsKeyCompletionItemFactory();
            var variableItemFactory = new VariableCompletionItemFactory();
            var uiItemFactory = new UICompletionItemFactory( snippetTextBuilder );
            var commandItemFactory = new CommandCompletionItemFactory( snippetTextBuilder );
            var callbackItemFactory = new CallbackCompletionItemFactory( snippetTextBuilder );
            var userFunctionItemFactory = new UserFunctionCompletionItemFactory( snippetTextBuilder );

            BuildCompletionItemNew( preprocessors, preprocessorItemFactory, word, preferSnippetInsertion, completions );
            BuildCompletionItemNew( pgsKeyIds, pgsItemFactory, word, preferSnippetInsertion, completions );
            BuildCompletionItemNew( userVariables, variableItemFactory, word, preferSnippetInsertion, completions );
            BuildCompletionItemNew( builtInVariables, variableItemFactory, word, preferSnippetInsertion, completions );
            BuildCompletionItemNew( uiTypes, uiItemFactory, word, preferSnippetInsertion, completions );
            BuildCompletionItemNew( commands, commandItemFactory, word, preferSnippetInsertion, completions );
            BuildCompletionItemNew( userFunctions, userFunctionItemFactory, word, preferSnippetInsertion, completions );
            BuildCompletionItemNew( builtInCallBacks, callbackItemFactory, word, preferSnippetInsertion, completions );
            #endregion ~Build completion list

            await Task.CompletedTask;

            return Result<CompletionHandlingOutput, LanguageServerFailureReason>.Success( new CompletionHandlingOutput( completions ) );
        }
        catch( Exception e )
        {
            return Result<CompletionHandlingOutput, LanguageServerFailureReason>.Failure( LanguageServerFailureReason.Other, e );
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
}
