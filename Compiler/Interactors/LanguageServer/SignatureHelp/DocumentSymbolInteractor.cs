using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.UseCases.LanguageServer.Symbol;

namespace KSPCompiler.Interactors.LanguageServer.SignatureHelp;

public sealed class DocumentSymbolInteractor : IDocumentSymbolUseCase
{
    public async Task<DocumentSymbolOutputPort> ExecuteAsync( DocumentSymbolInputPort parameter, CancellationToken cancellationToken = default )
    {
        try
        {
            var compilationCacheManager = parameter.HandlingInputData.Cache;
            var scriptLocation = parameter.HandlingInputData.Location;

            var cache = compilationCacheManager.GetCache( scriptLocation );
            var symbolTable = cache.SymbolTable;
            var result = new List<DocumentSymbol>();

            await CollectVariablesAsync( symbolTable.UserVariables, result );
            await CollectCallbackAsync( symbolTable.UserCallbacks, result );
            await CollectUserFunctionAsync( symbolTable.UserFunctions, result );

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

    private static async Task CollectVariablesAsync( IVariableSymbolTable symbolTable, List<DocumentSymbol> result )
    {
        foreach( var variable in symbolTable )
        {
            var detail = variable.DataType.ToMessageString();
            var kind = SymbolKind.Variable;

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

    private static async Task CollectCallbackAsync( ICallbackSymbolTable symbolTable, List<DocumentSymbol> result )
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

    private static async Task CollectUserFunctionAsync( IUserFunctionSymbolSymbolTable symbolTable, List<DocumentSymbol> result )
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
