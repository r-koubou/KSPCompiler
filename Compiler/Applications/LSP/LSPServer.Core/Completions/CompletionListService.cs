using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.LSPServer.Core.Compilations;

using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.LSPServer.Core.Completions;

public sealed class CompletionListService
{
    // ReSharper disable once MemberCanBeMadeStatic.Global
    public async Task<CompletionList> HandleAsync( CompilerCacheService compilerCacheService, CompletionParams request, CancellationToken cancellationToken )
    {
        var cache = compilerCacheService.GetCache( request.TextDocument.Uri );
        var symbolTable = cache.SymbolTable;
        var line = cache.AllLinesText[ request.Position.Line ];
        var word = DocumentUtility.ExtractWord( cache.AllLinesText, request.Position );
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
        BuildCompletionItem( preprocessors,    word, CompletionItemKind.Variable, "Preprocessor",      completions );
        BuildCompletionItem( userVariables,    word, CompletionItemKind.Variable, "User Variable",     completions );
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
        var stringBuilder = new StringBuilder(256);

        foreach( var symbol in symbols )
        {
            stringBuilder.Clear();

            if( symbol is CommandSymbol commandSymbol )
            {
                var completionItem = BuildCommandSnippet( commandSymbol, stringBuilder );

                target.Add( completionItem );
                continue;
            }

            if( symbol is VariableSymbol variableSymbol )
            {
                var item = BuildVariableItem( variableSymbol, partialName );

                target.Add( item );
                continue;
            }

            target.Add(
                new CompletionItem
                {
                    Label            = symbol.Name.Value,
                    Kind             = kind,
                    Detail           = detail,
                    InsertTextFormat = InsertTextFormat.PlainText,
                    InsertText       = symbol.Name.Value
                }
            );
        }
    }

    private static CompletionItem BuildVariableItem( VariableSymbol variableSymbol, string partialName )
    {
        var insertText = variableSymbol.Name.Value;
        var kind = CompletionItemKind.Variable;

        if( !variableSymbol.BuiltIn && variableSymbol.Modifier.IsConstant() )
        {
            kind = CompletionItemKind.Constant;
        }

        if( DataTypeUtility.StartsWithDataTypeCharacter( partialName ) )
        {
            insertText = variableSymbol.Name.Value[ 1.. ];
        }

        return new CompletionItem
        {
            Label            = variableSymbol.Name.Value,
            Kind             = kind,
            Detail           = variableSymbol.BuiltIn ? "Built-in Variable" : "User Variable",
            InsertTextFormat = InsertTextFormat.PlainText,
            InsertText       = insertText
        };
    }

    private static CompletionItem BuildCommandSnippet( CommandSymbol commandSymbol, StringBuilder stringBuilder )
    {
        if( commandSymbol.Arguments.Count == 0 )
        {
            return new CompletionItem
            {
                Label            = commandSymbol.Name.Value,
                Kind             = CompletionItemKind.Method,
                Detail           = "Command",
                InsertTextFormat = InsertTextFormat.PlainText,
                InsertText       = commandSymbol.Name.Value
            };
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

        return new CompletionItem
        {
            Label            = commandSymbol.Name.Value,
            Kind             = CompletionItemKind.Method,
            Detail           = "Command",
            InsertTextFormat = InsertTextFormat.Snippet,
            InsertText       = stringBuilder.ToString()
        };
    }
}
