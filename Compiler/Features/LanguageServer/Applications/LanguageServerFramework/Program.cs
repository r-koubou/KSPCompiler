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
using KSPCompiler.Features.Compilation.Infrastructures.BuiltInSymbolLoader.Yaml;
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
        var builtinSymbolLoader = CreateBuiltinSymbolLoader();

        server.AddHandler(
            new TextDocumentHandler(
                server,
                compilationCacheManager,
                builtinSymbolLoader
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
                builtinSymbolLoader
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
