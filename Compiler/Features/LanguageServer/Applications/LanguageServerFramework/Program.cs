using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Commands;
using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Completion;
using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Definition;
using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.FindReferences;
using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Folding;
using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Hover;
using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Renaming;
using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.SignatureHelp;
using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Symbols;
using KSPCompiler.Features.Compilation.Gateways.Symbols;
using KSPCompiler.Features.Compilation.Infrastructures.SymbolRepository.Yaml.Callbacks;
using KSPCompiler.Features.Compilation.Infrastructures.SymbolRepository.Yaml.Commands;
using KSPCompiler.Features.Compilation.Infrastructures.SymbolRepository.Yaml.UITypes;
using KSPCompiler.Features.Compilation.Infrastructures.SymbolRepository.Yaml.Variables;
using KSPCompiler.Features.LanguageServer.UseCase.Compilation;

namespace KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework;

public sealed class Program
{
    public static async Task Main( string[] args )
    {
        var input = Console.OpenStandardInput();
        var output = Console.OpenStandardOutput();

        var server = EmmyLua.LanguageServer.Framework.Server.LanguageServer.From( input, output );
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

        server.AddHandler(
            new ObfuscationCommandExecutor(
                server,
                compilationCacheManager,
                symbolRepositories
            )
        );

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
