using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Symbol;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData.Extensions;

namespace KSPCompiler.Features.LanguageServer.UseCase.DocumentSymbols;

public sealed class DocumentSymbolInteractor : IDocumentSymbolUseCase
{
    public async Task<DocumentSymbolOutputPort> ExecuteAsync( DocumentSymbolInputPort parameter, CancellationToken cancellationToken = default )
    {
        try
        {
            var compilationCacheManager = parameter.Input.Cache;
            var scriptLocation = parameter.Input.Location;

            var cache = compilationCacheManager.GetCache( scriptLocation );
            var symbolTable = cache.SymbolTable;
            var result = new List<DocumentSymbol>();

            var variableSymbols = new List<DocumentSymbol>();
            var uiVariableSymbols = new List<DocumentSymbol>();
            var callbackSymbols = new List<DocumentSymbol>();
            var userFunctions = new List<DocumentSymbol>();

            #region Variable
            await CollectVariablesAsync( symbolTable.UserVariables, variableSymbols, cancellationToken );

            if( variableSymbols.Any() )
            {
                // init コールバック内で children に設定するためここではソートのみ
                DocumentSymbolSorter.SortByOccurrence( variableSymbols );
            }
            #endregion ~Variable

            #region UI Variable
            await CollectUiVariablesAsync( symbolTable.UserVariables, uiVariableSymbols, cancellationToken );

            if( uiVariableSymbols.Any() )
            {
                // init コールバック内で children に設定するためここではソートのみ
                DocumentSymbolSorter.SortByOccurrence( uiVariableSymbols );
            }
            #endregion ~UI Variable

            #region Callback
            await CollectCallbackAsync(
                symbolTable.UserCallbacks,
                variableSymbols,
                uiVariableSymbols,
                callbackSymbols,
                cancellationToken
            );

            if( callbackSymbols.Any() )
            {
                result.AddRange( callbackSymbols );
            }
            #endregion ~Callback

            #region User Function
            await CollectUserFunctionAsync( symbolTable.UserFunctions, userFunctions, cancellationToken );

            if( userFunctions.Any() )
            {
                result.AddRange( userFunctions );
            }
            #endregion ~User Function

            DocumentSymbolSorter.SortByOccurrence( result );

            return new DocumentSymbolOutputPort( result, true );
        }
        catch( Exception e )
        {
            return new DocumentSymbolOutputPort( [ ], false, e );
        }
    }

    private static string GetArgumentDetailText<TArgumentSymbol>( IArgumentSymbolList<TArgumentSymbol> arguments, StringBuilder builder )
        where TArgumentSymbol : ArgumentSymbol
    {
        var argumentCount = arguments.Count;

        builder.Clear();

        for( var i = 0; i < argumentCount; i++ )
        {
            var argument = arguments[i];
            builder.Append( argument.Name.Value );

            if( i < argumentCount - 1 )
            {
                builder.Append( ", " );
            }
        }

        return builder.ToString();
    }

    private static async Task CollectVariablesAsync(
        IVariableSymbolTable symbolTable,
        List<DocumentSymbol> result,
        CancellationToken cancellationToken = default )
    {
        foreach( var variable in symbolTable )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var detail = variable.DataType.ToMessageString();
            var kind = SymbolKind.Variable;

            if( variable.Modifier.IsUI() )
            {
                continue;
            }

            if( variable.Modifier.IsConstant() )
            {
                kind = SymbolKind.Constant;
            }

            if( variable.UIType != UITypeSymbol.Null )
            {
                detail = variable.UIType.Name;
            }

            result.Add( new DocumentSymbol
                {
                    Name           = variable.Name,
                    Detail         = detail,
                    Kind           = kind,
                    Range          = variable.DefinedPosition,
                    SelectionRange = variable.DefinedPosition
                }
            );
        }

        await Task.CompletedTask;
    }

    private static async Task CollectUiVariablesAsync(
        IVariableSymbolTable symbolTable,
        List<DocumentSymbol> result,
        CancellationToken cancellationToken = default )
    {
        foreach( var variable in symbolTable )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var detail = variable.DataType.ToMessageString();
            var kind = SymbolKind.Variable;

            if( !variable.Modifier.IsUI() )
            {
                continue;
            }

            if( variable.Modifier.IsConstant() )
            {
                kind = SymbolKind.Constant;
            }

            if( variable.UIType != UITypeSymbol.Null )
            {
                detail = variable.UIType.Name;
            }

            result.Add( new DocumentSymbol
                {
                    Name           = variable.Name,
                    Detail         = detail,
                    Kind           = kind,
                    Range          = variable.DefinedPosition,
                    SelectionRange = variable.DefinedPosition
                }
            );
        }

        await Task.CompletedTask;
    }

    private static async Task CollectCallbackAsync(
        ICallbackSymbolTable symbolTable,
        List<DocumentSymbol> variableSymbols,
        List<DocumentSymbol> uiVariableSymbols,
        List<DocumentSymbol> result,
        CancellationToken cancellationToken = default )
    {
        var detailBuilder = new StringBuilder();

        foreach( var callback in symbolTable.ToList() )
        {
            cancellationToken.ThrowIfCancellationRequested();

            List<DocumentSymbol>? children = null;
            var detail = GetArgumentDetailText( callback.Arguments, detailBuilder );

            if( callback.Name == "init" )
            {
                children = new List<DocumentSymbol>();
                children.AddRange( variableSymbols );
                children.AddRange( uiVariableSymbols );
            }

            result.Add( new DocumentSymbol
                {
                    Name           = callback.Name,
                    Detail         = detail,
                    Kind           = SymbolKind.Event,
                    Range          = callback.Range,
                    SelectionRange = callback.DefinedPosition,
                    Children       = children
                }
            );
        }

        await Task.CompletedTask;
    }

    private static async Task CollectUserFunctionAsync(
        IUserFunctionSymbolSymbolTable symbolTable,
        List<DocumentSymbol> result,
        CancellationToken cancellationToken = default )
    {
        foreach( var function in symbolTable )
        {
            cancellationToken.ThrowIfCancellationRequested();

            result.Add( new DocumentSymbol
                {
                    Name           = function.Name,
                    Kind           = SymbolKind.Function,
                    Range          = function.Range,
                    SelectionRange = function.DefinedPosition
                }
            );
        }

        await Task.CompletedTask;
    }

}
