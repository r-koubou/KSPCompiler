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

public sealed class YamlBuiltInSymbolLoader(
    string baseDirectory,
    string variablesFileName = "variables.yaml",
    string uiTypesFileName = "uitypes.yaml",
    string commandsFileName = "commands.yaml",
    string callbacksFileName = "callbacks.yaml"
) : IBuiltInSymbolLoader
{
    private string VariablesFilePath { get; } = Path.Combine( baseDirectory, variablesFileName );
    private string UITypesFilePath { get; } = Path.Combine( baseDirectory, uiTypesFileName );
    private string CommandsFilePath { get; } = Path.Combine( baseDirectory, commandsFileName );
    private string CallbacksFilePath { get; } = Path.Combine( baseDirectory, callbacksFileName );

    public async Task<AggregateSymbolTable> LoadAsync( CancellationToken cancellationToken = default )
    {
        var variables = await LoadVariableSymbolsAsync( cancellationToken );
        var uiTypes = await LoadUITypeSymbolsAsync( cancellationToken );
        var commands = await LoadCommandSymbolsAsync( cancellationToken );
        var callbacks = await LoadCallbackSymbolsAsync( cancellationToken );

        return new AggregateSymbolTable(
            builtInVariables: variables,
            uiTypes: uiTypes,
            commandsNew: commands,
            builtInCallbacks: callbacks
        );
    }

    private async Task<VariableSymbolTable> LoadVariableSymbolsAsync( CancellationToken cancellationToken = default )
    {
        var contentReader = new LocalTextContentReader( VariablesFilePath );
        var symbolReader = new YamlVariableSymbolImporter( contentReader );
        var symbols = await symbolReader.ImportAsync( cancellationToken );
        var symbolTable = new VariableSymbolTable();
        symbolTable.AddRange( symbols );

        return symbolTable;
    }

    private async Task<UITypeSymbolTable> LoadUITypeSymbolsAsync( CancellationToken cancellationToken = default )
    {
        var contentReader = new LocalTextContentReader( UITypesFilePath );
        var symbolReader = new YamlUITypeSymbolImporter( contentReader );
        var symbols = await symbolReader.ImportAsync( cancellationToken );
        var symbolTable = new UITypeSymbolTable();
        symbolTable.AddRange( symbols );

        return symbolTable;
    }

    private async Task<CommandSymbolTable> LoadCommandSymbolsAsync( CancellationToken cancellationToken = default )
    {
        var contentReader = new LocalTextContentReader( CommandsFilePath );
        var symbolReader = new YamlCommandSymbolImporter( contentReader );
        var symbols = await symbolReader.ImportAsync( cancellationToken );
        var symbolTable = new CommandSymbolTable();

        foreach( var x in symbols )
        {
            symbolTable.AddAsOverload( x, x.Arguments );
        }

        return symbolTable;
    }

    private async Task<CallbackSymbolTable> LoadCallbackSymbolsAsync( CancellationToken cancellationToken = default )
    {
        var contentReader = new LocalTextContentReader( CallbacksFilePath );
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
