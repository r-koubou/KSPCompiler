using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Client.ClientCapabilities;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server;
using EmmyLua.LanguageServer.Framework.Protocol.Message.ExecuteCommand;
using EmmyLua.LanguageServer.Framework.Server.Handler;

using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Compilation;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Compilation;

namespace KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Commands;

public class ObfuscationCommandExecutor(
    ICompilationCacheManager compilationCacheManager,
    CompilationServerService compilationServerService
) : ExecuteCommandHandlerBase
{
    private const string CommandName = "ksp.obfuscate";

    private readonly ICompilationCacheManager compilationCacheManager = compilationCacheManager;
    private readonly CompilationServerService compilationSeverService = compilationServerService;

    protected override async Task<ExecuteCommandResponse> Handle( ExecuteCommandParams request, CancellationToken token )
    {
        if( request.Command != CommandName )
        {
            return new ExecuteCommandResponse( null );
        }

        var documentUri = request.Arguments?[ 0 ].Value?.ToString();
        _ = documentUri ?? throw new ArgumentException( $"{nameof( documentUri )} is null" );

        if( !ScriptLocation.TryParse( documentUri, out var scriptLocation ) )
        {
            return new ExecuteCommandResponse( null );
        }

        var script = await File.ReadAllTextAsync( scriptLocation.FileSystemPath, token );

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
