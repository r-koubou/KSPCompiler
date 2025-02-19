using System.IO;
using System.Threading.Tasks;

using KSPCompiler.Applications.LSPServer.Core.Compilations;
using KSPCompiler.Applications.LSPServer.Core.Completions;
using KSPCompiler.Applications.LSPServer.Core.Definitions;
using KSPCompiler.Applications.LSPServer.Core.DocumentHighlights;
using KSPCompiler.Applications.LSPServer.Core.ExecuteCommands;
using KSPCompiler.Applications.LSPServer.Core.Foldings;
using KSPCompiler.Applications.LSPServer.Core.Hovers;
using KSPCompiler.Applications.LSPServer.Core.References;
using KSPCompiler.Applications.LSPServer.Core.Renames;
using KSPCompiler.Applications.LSPServer.Core.SignatureHelps;
using KSPCompiler.Applications.LSPServer.Core.Symbols;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using OmniSharp.Extensions.LanguageServer.Protocol.Window;
using OmniSharp.Extensions.LanguageServer.Server;

namespace KSPCompiler.Applications.LSPServer.Core;

public class Server
{
    public class Option
    {
        public Stream Input { get; }
        public Stream Output { get; }
        public ILoggerFactory? LoggerFactory { get; }
        public LogLevel MinimumLevel { get; }

        public Option(
            Stream input,
            Stream output,
            ILoggerFactory? loggerFactory = null,
            LogLevel minimumLevel = LogLevel.Information )
        {
            Input         = input;
            Output        = output;
            LoggerFactory = loggerFactory;
            MinimumLevel  = minimumLevel;
        }
    }

    public static async Task<LanguageServer> CreateAsync( Option option )
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
                      .WithLoggerFactory( option.LoggerFactory ?? NullLoggerFactory.Instance )
                      .WithHandler<TextDocumentHandler>()
                      .WithHandler<DefinitionHandler>()
                      .WithHandler<DocumentSymbolHandler>()
                      .WithHandler<DocumentHighlightHandler>()
                      .WithHandler<CompletionHandler>()
                      .WithHandler<CompletionResolveHandler>()
                      .WithHandler<HoverHandler>()
                      .WithHandler<RenameHandler>()
                      .WithHandler<PrepareRenameHandler>()
                      .WithHandler<ReferencesHandler>()
                      .WithHandler<FoldingRangeHandler>()
                      .WithHandler<SignatureHelpHandler>()
                      .WithHandler<ObfuscationCommandHandler>()
                      .WithServices(
                           services =>
                           {
                               services.AddSingleton<EventEmittingService>();
                               services.AddSingleton<LspCompilationService>();
                               services.AddSingleton<CompilerCacheService>();
                               services.AddSingleton<DefinitionService>();
                               services.AddSingleton<SymbolInformationService>();
                               services.AddSingleton<DocumentHighlightService>();
                               services.AddSingleton<CompletionListService>();
                               services.AddSingleton<HoverService>();
                               services.AddSingleton<RenameService>();
                               services.AddSingleton<ReferencesService>();
                               services.AddSingleton<FoldingRangeService>();
                               services.AddSingleton<SignatureHelpService>();
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
