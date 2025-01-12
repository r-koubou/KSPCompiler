using System.IO;
using System.Threading.Tasks;

using KSPCompiler.LSPServer.Core.Compilations;
using KSPCompiler.LSPServer.Core.Completions;
using KSPCompiler.LSPServer.Core.Definitions;
using KSPCompiler.LSPServer.Core.DocumentHighlights;
using KSPCompiler.LSPServer.Core.Hovers;
using KSPCompiler.LSPServer.Core.Renames;
using KSPCompiler.LSPServer.Core.Symbols;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OmniSharp.Extensions.LanguageServer.Protocol.Window;
using OmniSharp.Extensions.LanguageServer.Server;

namespace KSPCompiler.LSPServer.Core;

public class Server
{
    public class Option
    {
        public Stream Input { get;  }
        public Stream Output { get; }
        public LogLevel MinimumLevel { get; }

        public Option( Stream input, Stream output, LogLevel minimumLevel )
        {
            Input    = input;
            Output   = output;
            MinimumLevel = minimumLevel;
        }
    }

    public static async Task<LanguageServer> Create( Option option )
    {
        return await LanguageServer.From(
            options => options
                      .WithInput( option.Input )
                      .WithOutput( option.Output )
                      .ConfigureLogging( logging =>
                           {
                               logging.SetMinimumLevel( option.MinimumLevel );
                           }
                       )
                      .WithHandler<TextDocumentHandler>()
                      .WithHandler<DefinitionHandler>()
                      .WithHandler<DocumentSymbolHandler>()
                      .WithHandler<DocumentHighlightHandler>()
                      .WithHandler<CompletionHandler>()
                      .WithHandler<CompletionResolveHandler>()
                      .WithHandler<HoverHandler>()
                      .WithHandler<RenameHandler>()
                      .WithServices(
                           services =>
                           {
                               services.AddSingleton<CompilationService>();
                               services.AddSingleton<CompilerCacheService>();
                               services.AddSingleton<DefinitionService>();
                               services.AddSingleton<SymbolInformationService>();
                               services.AddSingleton<DocumentHighlightService>();
                               services.AddSingleton<CompletionListService>();
                               services.AddSingleton<HoverService>();
                               services.AddSingleton<RenameService>();
                           }
                       )
                      .OnInitialize( ( server, request, token ) =>
                           {
                               server.LogInfo( "Server initialized." );
                               return Task.CompletedTask;
                           }
                       )
                      .OnStarted( ( server, token ) =>
                           {
                               server.LogInfo( "Server started." );
                               return Task.CompletedTask;
                           }
                       )
        );
    }
}
