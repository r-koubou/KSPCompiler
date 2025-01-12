using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.LSPServer.Core.Compilations;
using KSPCompiler.LSPServer.Core.Extensions;

using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.LSPServer.Core.Symbols;

public sealed class SymbolInformationService
{
    private static readonly IReadOnlyDictionary<DataTypeFlag, string> DataTypeTextMap = new Dictionary<DataTypeFlag, string>
    {
        { DataTypeFlag.None, "unknown" },
        { DataTypeFlag.TypeInt, "integer" },
        { DataTypeFlag.TypeString, "string" },
        { DataTypeFlag.TypeReal, "real" },
        { DataTypeFlag.TypePreprocessorSymbol, "preprocessor" },
        { DataTypeFlag.TypePgsId, "Pgs" }
    };

    public static string GetTypeText( DataTypeFlag flag )
    {
        var typeMasked = flag.TypeMasked();
        var isArray = flag.IsArray();

        if( DataTypeTextMap.TryGetValue( flag, out var messageText ) )
        {
            return messageText;
        }

        var result = "";
        var foundCount = 0;

        foreach( var (k,v) in DataTypeTextMap )
        {
            var type = typeMasked & k;
            if( type == 0 )
            {
                continue;
            }

            if( foundCount > 0 )
            {
                result += " / ";
            }

            result += $"{v}{( isArray ? "[]" : string.Empty )}";
            foundCount++;
        }

        return result;
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
        foreach( var variable in symbolTable )
        {
            result.Add( new DocumentSymbol
                {
                    Name           = variable.Name,
                    Kind           = SymbolKind.Event,
                    Range          = variable.DefinedPosition.AsRange(),
                    SelectionRange = variable.DefinedPosition.AsRange()
                }
            );
        }

        await Task.CompletedTask;
    }

    private static async Task CollectUserFunctionAsync( IUserFunctionSymbolSymbolTable symbolTable, List<SymbolInformationOrDocumentSymbol> result )
    {
        foreach( var variable in symbolTable )
        {
            result.Add( new DocumentSymbol
                {
                    Name           = variable.Name,
                    Kind           = SymbolKind.Function,
                    Range          = variable.DefinedPosition.AsRange(),
                    SelectionRange = variable.DefinedPosition.AsRange()
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
