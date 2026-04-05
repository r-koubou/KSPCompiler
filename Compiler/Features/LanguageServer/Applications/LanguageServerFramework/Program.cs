using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Commands;
using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Compilation;
using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Completion;
using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Definition;
using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.FindReferences;
using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Folding;
using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Hover;
using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Renaming;
using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.SignatureHelp;
using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Symbols;
using KSPCompiler.Features.Compilation.Gateways.Symbol;
using KSPCompiler.Features.Compilation.Infrastructures.BuiltInSymbolLoader.Yaml;
using KSPCompiler.Features.Compilation.UseCase.ApplicationServices;
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
                var version = typeof( Program ).Assembly.GetName().Version;
                serverInfo.Name = "ksp";
                serverInfo.Version = version is null
                    ? "unknown"
                    : $"{version.Major}.{version.Minor}.{version.Build}";

                await Console.Error.WriteLineAsync( "Server#OnInitialize" );
                await Console.Error.WriteLineAsync( $"Server Version: {serverInfo.Version}" );
            }
        );
        server.OnInitialized( async initializedParams =>
            {
                await Console.Error.WriteLineAsync( "Server#OnInitialized" );
            }
        );

        #region Parse command line arguments
        var preferSnippetInsertion = false;

#if DEBUG
        await Console.Error.WriteLineAsync( "Command line arguments:" );

        foreach( var arg in args )
        {
            await Console.Error.WriteLineAsync( arg );
        }
#endif

        foreach( var arg in args )
        {
            if( arg == "--prefer-snippet-insertion" )
            {
                preferSnippetInsertion = true;
            }
        }
        #endregion

        #region Register Handlers
        var compilationCacheManager = new CompilationCacheManager();
        var builtinSymbolLoader = CreateBuiltinSymbolLoader();
        var builtinSymbolTable = await builtinSymbolLoader.LoadAsync();

        var compilationRequestHandler = new CompilationRequestHandler();
        var compilationMediator = new CompilationMediator( compilationRequestHandler );

        var compilationSeverService = new CompilationServerService(
            server.Client,
            compilationMediator,
            builtinSymbolTable
        );

        server.AddHandler(
            new TextDocumentHandler(
                compilationCacheManager,
                compilationSeverService
            )
        );
        server.AddHandler(
            new CompletionHandler(
                compilationCacheManager, new CompletionHandlerConfig
                {
                    PreferSnippetInsertion = preferSnippetInsertion
                }
            )
        );
        server.AddHandler( new DefinitionHandler( compilationCacheManager ) );
        server.AddHandler( new FoldingRabgeHandler( compilationCacheManager ) );
        server.AddHandler( new HoverHandler( compilationCacheManager ) );
        server.AddHandler( new ReferencesHandler( compilationCacheManager ) );
        server.AddHandler( new RenameHandler( compilationCacheManager ) );
        server.AddHandler( new SignatureHelpHandler( compilationCacheManager ) );
        server.AddHandler( new DocumentSymbolHandler( compilationCacheManager ) );

        server.AddHandler(
            new ObfuscationCommandExecutor(
                compilationCacheManager,
                compilationSeverService
            )
        );

        #endregion ~Register Handlers

        await server.Run();
    }

    #region Setup Symbols
    private static IBuiltInSymbolLoader CreateBuiltinSymbolLoader()
    {
        var baseDir = Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location ) ?? ".";
        var basePath = Path.Combine( baseDir, "Data", "Symbols" );
        return new YamlBuiltInSymbolLoader( basePath );
    }
    #endregion
}
