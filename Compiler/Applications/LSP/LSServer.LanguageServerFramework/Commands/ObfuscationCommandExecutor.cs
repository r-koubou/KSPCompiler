using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Client.ClientCapabilities;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server;
using EmmyLua.LanguageServer.Framework.Protocol.Message.ExecuteCommand;
using EmmyLua.LanguageServer.Framework.Server;
using EmmyLua.LanguageServer.Framework.Server.Handler;

using KSPCompiler.Applications.LSServer.LanguageServerFramework.Compilation;
using KSPCompiler.Applications.LSServer.LanguageServerFramework.Extensions;
using KSPCompiler.Gateways.Symbols;
using KSPCompiler.Interactors.ApplicationServices.Compilation;
using KSPCompiler.Interactors.ApplicationServices.Symbols;
using KSPCompiler.Interactors.Symbols;
using KSPCompiler.UseCases.LanguageServer;
using KSPCompiler.UseCases.LanguageServer.Compilation;

namespace KSPCompiler.Applications.LSServer.LanguageServerFramework.Commands;

public class ObfuscationCommandExecutor(
    LanguageServer server,
    ICompilationCacheManager compilationCacheManager,
    AggregateSymbolRepository symbolRepositories

) : ExecuteCommandHandlerBase
{
    private const string CommandName = "ksp.obfuscate";

    private readonly ICompilationCacheManager compilationCacheManager = compilationCacheManager;

    private readonly CompilationServerService compilationSeverService = new(
        server.Client,
        new CompilationApplicationService(
            new LoadingBuiltinSymbolApplicationService(
                new LoadBuiltinSymbolInteractor(),
                symbolRepositories
            )
        )
    );

    protected override async Task<ExecuteCommandResponse> Handle( ExecuteCommandParams request, CancellationToken token )
    {
        if( request.Command != CommandName )
        {
            return new ExecuteCommandResponse( null );
        }


        var uri = request.Arguments?[ 0 ].Value?.ToString();
        var scriptLocation = new ScriptLocation( uri! ).RemoveFileSchemeString();

        if( !compilationCacheManager.ContainsCache( scriptLocation ) )
        {
            return new ExecuteCommandResponse( string.Empty );
        }

        var script = await File.ReadAllTextAsync( scriptLocation.Value, token );

        var result = await compilationSeverService.CompileAsync(
            compilationCacheManager: compilationCacheManager,
            scriptLocation: scriptLocation,
            script: script,
            enableObfuscation: true,
            cancellationToken: token
        );

        if( !result.Result )
        {
            return new ExecuteCommandResponse( null );
        }

        return new ExecuteCommandResponse( result.ObfuscatedScript );
    }

    public override void RegisterCapability( ServerCapabilities serverCapabilities, ClientCapabilities clientCapabilities ) {}
}
