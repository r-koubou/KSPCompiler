using System.IO;
using System.Threading.Tasks;

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
                      .WithServices(
                           services =>
                           {
                               services.AddSingleton<CompilationService>();
                               services.AddSingleton<CompilerCache>();
                               services.AddSingleton<SymbolInformationService>();
                               services.AddSingleton<CompletionListService>();
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
