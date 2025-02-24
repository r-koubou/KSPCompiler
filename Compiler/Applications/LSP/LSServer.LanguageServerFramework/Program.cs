using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using EmmyLua.LanguageServer.Framework.Server;

using KSPCompiler.Applications.LSPServer.Core.Compilation;
using KSPCompiler.Applications.LSServer.LanguageServerFramework.Completion;
using KSPCompiler.Applications.LSServer.LanguageServerFramework.Definition;
using KSPCompiler.Applications.LSServer.LanguageServerFramework.FindReferences;
using KSPCompiler.Applications.LSServer.LanguageServerFramework.Folding;
using KSPCompiler.Applications.LSServer.LanguageServerFramework.Hover;
using KSPCompiler.Applications.LSServer.LanguageServerFramework.Renaming;
using KSPCompiler.Applications.LSServer.LanguageServerFramework.SignatureHelp;
using KSPCompiler.Applications.LSServer.LanguageServerFramework.Symbols;
using KSPCompiler.ExternalSymbolRepository.Yaml.Callbacks;
using KSPCompiler.ExternalSymbolRepository.Yaml.Commands;
using KSPCompiler.ExternalSymbolRepository.Yaml.UITypes;
using KSPCompiler.ExternalSymbolRepository.Yaml.Variables;
using KSPCompiler.Gateways.Symbols;

namespace KSPCompiler.Applications.LSServer.LanguageServerFramework;

public sealed class Program
{
    public static async Task Main( string[] args )
    {
        var input = Console.OpenStandardInput();
        var output = Console.OpenStandardOutput();

        var server = LanguageServer.From( input, output );
        server.OnInitialize( async ( initializeParams, serverInfo ) =>
            {
                serverInfo.Name    = "ksp";
                serverInfo.Version = "1.0.0";
                await Console.Error.WriteLineAsync( "Server#OnInitialize" );
            }
        );
        server.OnInitialize( async ( c, s ) =>
            {
                await Console.Error.WriteLineAsync( "Server#OnInitialized" );
            }
        );

        #region Register Handlers
        var compilationCacheManager = new CompilationCacheManager();
        using var symbolRepositories = CreateSymbolRepositories();

        server.AddHandler(
            new TextDocumentHandler(
                server,
                compilationCacheManager,
                symbolRepositories
            )
        );
        server.AddHandler( new CompletionHandler( compilationCacheManager ) );
        server.AddHandler( new DefinitionHandler( compilationCacheManager ) );
        server.AddHandler( new FoldingRabgeHandler( compilationCacheManager ) );
        server.AddHandler( new HoverHandler( compilationCacheManager ) );
        server.AddHandler( new ReferencesHandler( compilationCacheManager ) );
        server.AddHandler( new RenameHandler( compilationCacheManager ) );
        server.AddHandler( new SignatureHelpHandler( compilationCacheManager ) );
        server.AddHandler( new DocumentSymbolHandler( compilationCacheManager ) );
        #endregion ~Register Handlers

        await server.Run();
    }

    #region Setup Symbols
    private static AggregateSymbolRepository CreateSymbolRepositories()
    {
        var baseDir = Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location ) ?? ".";
        var basePath = Path.Combine( baseDir, "Data", "Symbols" );

        return new AggregateSymbolRepository(
            new VariableSymbolRepository( Path.Combine( basePath, "variables.yaml" ) ),
            new UITypeSymbolRepository( Path.Combine( basePath, "uitypes.yaml" ) ),
            new CommandSymbolRepository( Path.Combine( basePath, "commands.yaml" ) ),
            new CallbackSymbolRepository( Path.Combine( basePath, "callbacks.yaml" )
            )
        );
    }
    #endregion
}
