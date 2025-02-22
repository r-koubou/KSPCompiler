using System;
using System.Threading.Tasks;

using EmmyLua.LanguageServer.Framework.Server;

using KSPCompiler.Applications.LSPServer.CoreNew.Compilation;
using KSPCompiler.Applications.LSServer.LanguageServerFramework.Hover;

namespace KSPCompiler.Applications.LSServer.LanguageServerFramework;

public sealed class Program
{
    public static async Task Main( string[] args )
    {
        var input = Console.OpenStandardInput();
        var output = Console.OpenStandardOutput();

        var compilationCacheManager = new CompilationCacheManager();

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

        server.AddHandler( new TextDocumentHandler( compilationCacheManager ) );
        server.AddHandler( new HoverHandler( compilationCacheManager ) );

        await server.Run();
    }
}
