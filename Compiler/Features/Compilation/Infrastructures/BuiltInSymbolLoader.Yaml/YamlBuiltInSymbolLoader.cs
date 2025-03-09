using System.IO;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.Compilation.Gateways.Symbol;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Local;
using KSPCompiler.Shared.IO.Symbols.Yaml.Callbacks;
using KSPCompiler.Shared.IO.Symbols.Yaml.Commands;
using KSPCompiler.Shared.IO.Symbols.Yaml.UITypes;
using KSPCompiler.Shared.IO.Symbols.Yaml.Variables;

namespace KSPCompiler.Features.Compilation.Infrastructures.BuiltInSymbolLoader.Yaml;

public sealed class YamlBuiltInSymbolLoader( string baseDirectory ) : IBuiltInSymbolLoader
{
    private string BaseDirectory { get; } = baseDirectory;

    public async Task<AggregateSymbolTable> LoadAsync( CancellationToken cancellationToken = default )
    {
        var variables = await LoadVariableSymbolsAsync( cancellationToken );
        var uiTypes = await LoadUITypeSymbolsAsync( cancellationToken );
        var commands = await LoadCommandSymbolsAsync( cancellationToken );
        var callbacks = await LoadCallbackSymbolsAsync( cancellationToken );

        return new AggregateSymbolTable(
            builtInVariables: variables,
            uiTypes: uiTypes,
            commands: commands,
            builtInCallbacks: callbacks
        );
    }

    private async Task<VariableSymbolTable> LoadVariableSymbolsAsync( CancellationToken cancellationToken = default )
    {
        var filePath = Path.Combine( BaseDirectory, "variables.yaml" );
        var contentReader = new LocalTextContentReader( filePath );
        var symbolReader = new YamlVariableSymbolImporter( contentReader );
        var symbols = await symbolReader.ImportAsync( cancellationToken );
        var symbolTable = new VariableSymbolTable();
        symbolTable.AddRange( symbols );

        return symbolTable;
    }

    private async Task<UITypeSymbolTable> LoadUITypeSymbolsAsync( CancellationToken cancellationToken = default )
    {
        var filePath = Path.Combine( BaseDirectory, "uitypes.yaml" );
        var contentReader = new LocalTextContentReader( filePath );
        var symbolReader = new YamlUITypeSymbolImporter( contentReader );
        var symbols = await symbolReader.ImportAsync( cancellationToken );
        var symbolTable = new UITypeSymbolTable();
        symbolTable.AddRange( symbols );

        return symbolTable;
    }

    private async Task<CommandSymbolTable> LoadCommandSymbolsAsync( CancellationToken cancellationToken = default )
    {
        var filePath = Path.Combine( BaseDirectory, "commands.yaml" );
        var contentReader = new LocalTextContentReader( filePath );
        var symbolReader = new YamlCommandSymbolImporter( contentReader );
        var symbols = await symbolReader.ImportAsync( cancellationToken );
        var symbolTable = new CommandSymbolTable();
        symbolTable.AddRange( symbols );

        return symbolTable;
    }

    private async Task<CallbackSymbolTable> LoadCallbackSymbolsAsync( CancellationToken cancellationToken = default )
    {
        var filePath = Path.Combine( BaseDirectory, "callbacks.yaml" );
        var contentReader = new LocalTextContentReader( filePath );
        var symbolReader = new YamlCallbackSymbolImporter( contentReader );
        var symbols = await symbolReader.ImportAsync( cancellationToken );
        var symbolTable = new CallbackSymbolTable();

        foreach( var x in symbols )
        {
            if( x.AllowMultipleDeclaration )
            {
                symbolTable.AddAsOverload( x, x.Arguments );
            }
            else
            {
                symbolTable.AddAsNoOverload( x );
            }
        }

        return symbolTable;
    }
}
