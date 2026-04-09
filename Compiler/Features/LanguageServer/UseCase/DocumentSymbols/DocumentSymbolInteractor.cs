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

            #region Variable
            {
                var variableSymbols = new List<DocumentSymbol>();
                await CollectVariablesAsync( symbolTable.UserVariables, variableSymbols, cancellationToken );

                if( variableSymbols.Any() )
                {
                    var firstElement = variableSymbols.First();
                    var variableSymbolsRoot = new DocumentSymbol
                    {
                        Name           = "Variable",
                        Kind           = SymbolKind.Variable,
                        Range          = firstElement.Range,
                        SelectionRange = firstElement.SelectionRange,
                        Children       = variableSymbols
                    };

                    result.Add( variableSymbolsRoot );
                }
            }
            #endregion ~Variable

            #region UI Variable
            {
                var variableSymbols = new List<DocumentSymbol>();
                await CollectUiVariablesAsync( symbolTable.UserVariables, variableSymbols,  cancellationToken );

                if( variableSymbols.Any() )
                {
                    var firstElement = variableSymbols.First();
                    var variableSymbolsRoot = new DocumentSymbol
                    {
                        Name           = "UI",
                        Kind           = SymbolKind.Variable,
                        Range          = firstElement.Range,
                        SelectionRange = firstElement.SelectionRange,
                        Children       = variableSymbols
                    };

                    result.Add( variableSymbolsRoot );
                }
            }
            #endregion ~UI Variable

            #region Callback
            {
                var callbackSymbols = new List<DocumentSymbol>();
                await CollectCallbackAsync( symbolTable.UserCallbacks, callbackSymbols, cancellationToken );

                if( callbackSymbols.Any() )
                {
                    var firstElement = callbackSymbols.First();
                    var callbackSymbolsRoot = new DocumentSymbol
                    {
                        Name           = "Callback",
                        Kind           = SymbolKind.Event,
                        Range          = firstElement.Range,
                        SelectionRange = firstElement.SelectionRange,
                        Children       = callbackSymbols
                    };

                    result.Add( callbackSymbolsRoot );
                }
            }
            #endregion ~Callback

            #region User Function
            {
                var userFunctions = new List<DocumentSymbol>();
                await CollectUserFunctionAsync( symbolTable.UserFunctions, userFunctions, cancellationToken );

                if( userFunctions.Any() )
                {
                    var firstElement = userFunctions.First();
                    var userFunctionSymbolsRoot = new DocumentSymbol
                    {
                        Name           = "User Function",
                        Kind           = SymbolKind.Function,
                        Range          = firstElement.Range,
                        SelectionRange = firstElement.SelectionRange,
                        Children       = userFunctions
                    };

                    result.Add( userFunctionSymbolsRoot );
                }
            }
            #endregion ~User Function

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
        CancellationToken _ = default )
    {
        foreach( var variable in symbolTable )
        {
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
        CancellationToken _ = default )
    {
        foreach( var variable in symbolTable )
        {
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
        List<DocumentSymbol> result,
        CancellationToken _ = default )
    {
        var detailBuilder = new StringBuilder();

        foreach( var callback in symbolTable.ToList() )
        {
            var detail = GetArgumentDetailText( callback.Arguments, detailBuilder );

            result.Add( new DocumentSymbol
                {
                    Name           = callback.Name,
                    Detail         = detail,
                    Kind           = SymbolKind.Event,
                    Range          = callback.DefinedPosition,
                    SelectionRange = callback.DefinedPosition
                }
            );
        }

        await Task.CompletedTask;
    }

    private static async Task CollectUserFunctionAsync(
        IUserFunctionSymbolSymbolTable symbolTable,
        List<DocumentSymbol> result,
        CancellationToken _ = default )
    {
        foreach( var function in symbolTable )
        {
            result.Add( new DocumentSymbol
                {
                    Name           = function.Name,
                    Kind           = SymbolKind.Function,
                    Range          = function.DefinedPosition,
                    SelectionRange = function.DefinedPosition
                }
            );
        }

        await Task.CompletedTask;
    }

}
