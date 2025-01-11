using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;

using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.LSPServer.Core;

public class CompletionListService
{
    // ReSharper disable once MemberCanBeMadeStatic.Global
    public async Task<CompletionList> HandleAsync( CompilerCache compilerCache, CompletionParams request, CancellationToken cancellationToken )
    {
        var cache = compilerCache.GetCache( request.TextDocument.Uri );
        var symbolTable = cache.SymbolTable;
        var line = cache.AllLinesText[ request.Position.Line ];
        var word = DocumentUtility.ExtractWord( cache.AllLinesText, request.Position );
        var completions = new List<CompletionItem>();

        #region Collection of target symbols
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
            word,
            _ => line.Trim().StartsWith( "call" )
        );

        // ビルトインコールバック
        // ユーザー定義のコールバックについてははビルトインのコールバック名なので扱わない
        var builtInCallBacks = MatchCompletionItem(
            symbolTable.BuiltInCallbacks,
            word
        );
        #endregion ~Collection of target symbols

        #region Build completion list
        BuildCompletionItem( userVariables,    word, CompletionItemKind.Function, "User Variable",     completions );
        BuildCompletionItem( builtInVariables, word, CompletionItemKind.Function, "Built-in Variable", completions );
        BuildCompletionItem( uiTypes,          word, CompletionItemKind.Class,    "UI Type",           completions );
        BuildCompletionItem( commands,         word, CompletionItemKind.Method,   "Command",           completions );
        BuildCompletionItem( userFunctions,    word, CompletionItemKind.Function, "User Function",     completions );
        BuildCompletionItem( builtInCallBacks, word, CompletionItemKind.Function, "Callback",          completions );
        #endregion ~Build completion list

        await Task.CompletedTask;

        return new CompletionList( completions );
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

    private static void BuildCompletionItem<TSymbol>(
        IReadOnlyCollection<TSymbol> symbols,
        string partialName,
        CompletionItemKind kind,
        string detail,
        List<CompletionItem> target ) where TSymbol : SymbolBase
    {
        foreach( var symbol in symbols )
        {
            var insertText = symbol.Name.Value;

            if( DataTypeUtility.StartsWithDataTypeCharacter( partialName ) )
            {
                insertText = symbol.Name.Value[ 1.. ];
            }

            target.Add(
                new CompletionItem
                {
                    Label      = symbol.Name.Value,
                    Kind       = kind,
                    Detail     = detail,
                    InsertText = insertText
                }
            );
        }
    }
}
