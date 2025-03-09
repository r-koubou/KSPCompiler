using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Client.ClientCapabilities;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server;
using EmmyLua.LanguageServer.Framework.Protocol.Message.ExecuteCommand;
using EmmyLua.LanguageServer.Framework.Server.Handler;

using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Compilation;
using KSPCompiler.Features.Compilation.Gateways.Symbol;
using KSPCompiler.Features.Compilation.UseCase.ApplicationServices;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Compilation;

namespace KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Commands;

public class ObfuscationCommandExecutor(
    EmmyLua.LanguageServer.Framework.Server.LanguageServer server,
    ICompilationCacheManager compilationCacheManager,
    IBuiltInSymbolLoader builtInSymbolLoader
) : ExecuteCommandHandlerBase
{
    private const string CommandName = "ksp.obfuscate";

    private readonly ICompilationCacheManager compilationCacheManager = compilationCacheManager;

    private readonly CompilationServerService compilationSeverService = new(
        server.Client,
        new CompilationApplicationService( builtInSymbolLoader )
    );

    private static ScriptLocation ToScriptLocation( string documentUri )
    {
        var uri = new Uri( documentUri );
        var filePath = uri.LocalPath + Uri.UnescapeDataString( uri.Fragment );

        // Fix for Windows file system
        // Remove '/' from the start of the path if it is a drive letter
        if( filePath.StartsWith( '/' ) && filePath.Length > 2 && filePath[ 2 ] == ':' )
        {
            filePath = filePath[ 1.. ];
        }

        var fileInfo = new FileInfo( filePath );

        return new ScriptLocation( fileInfo.FullName );
    }

    protected override async Task<ExecuteCommandResponse> Handle( ExecuteCommandParams request, CancellationToken token )
    {
        if( request.Command != CommandName )
        {
            return new ExecuteCommandResponse( null );
        }

        var documentUri = request.Arguments?[ 0 ].Value?.ToString();
        _ = documentUri ?? throw new ArgumentException( $"{nameof( documentUri )} is null" );

        var scriptLocation = ToScriptLocation( documentUri! );
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
