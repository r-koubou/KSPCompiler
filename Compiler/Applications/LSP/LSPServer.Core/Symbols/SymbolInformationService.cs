using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Applications.LSPServer.Core.Compilations;
using KSPCompiler.Applications.LSPServer.Core.Extensions;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;

using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.Applications.LSPServer.Core.Symbols;

public sealed class SymbolInformationService
{
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

    private static async Task CollectVariablesAsync( IVariableSymbolTable symbolTable, List<SymbolInformationOrDocumentSymbol> result )
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
                    Range          = variable.DefinedPosition.AsRange(),
                    SelectionRange = variable.DefinedPosition.AsRange()
                }
            );
        }

        await Task.CompletedTask;
    }

    private static async Task CollectCallbackAsync( ICallbackSymbolTable symbolTable, List<SymbolInformationOrDocumentSymbol> result )
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
                    Range          = callback.DefinedPosition.AsRange(),
                    SelectionRange = callback.DefinedPosition.AsRange()
                }
            );
        }

        await Task.CompletedTask;
    }

    private static async Task CollectUserFunctionAsync( IUserFunctionSymbolSymbolTable symbolTable, List<SymbolInformationOrDocumentSymbol> result )
    {
        foreach( var function in symbolTable )
        {
            result.Add( new DocumentSymbol
                {
                    Name           = function.Name,
                    Kind           = SymbolKind.Function,
                    Range          = function.DefinedPosition.AsRange(),
                    SelectionRange = function.DefinedPosition.AsRange()
                }
            );
        }

        await Task.CompletedTask;
    }

    public async Task<SymbolInformationOrDocumentSymbolContainer?> HandleAsync( CompilerCacheService compilerCacheService, DocumentSymbolParams request, CancellationToken cancellationToken )
    {
        var cache = compilerCacheService.GetCache( request.TextDocument.Uri );
        var symbolTable = cache.SymbolTable;
        var result = new List<SymbolInformationOrDocumentSymbol>();

        await CollectVariablesAsync( symbolTable.UserVariables, result );
        await CollectCallbackAsync( symbolTable.UserCallbacks, result );
        await CollectUserFunctionAsync( symbolTable.UserFunctions, result );

        return result;
    }
}
