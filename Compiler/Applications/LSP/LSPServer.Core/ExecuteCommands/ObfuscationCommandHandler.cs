using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Applications.LSPServer.Core.Compilations;

using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Workspace;

namespace KSPCompiler.Applications.LSPServer.Core.ExecuteCommands;

/// <summary>
/// Implementation of Obfuscation Command
/// </summary>
/// <remarks>
/// <para>request.Arguments</para>
/// <list type="table">
///     <listheader>
///         <term>Index</term>
///         <description>Argument Index</description>
///     </listheader>
///     <item>
///         <term>0</term>
///         <description>Document Uri</description>
///     </item>
/// </list>
/// </remarks>
public class ObfuscationCommandHandler( LspCompilationService lspCompilationService ) : IExecuteCommandHandler<string>
{
    private const string ObfuscationCommand = "ksp.obfuscate";

    private LspCompilationService LspCompilationService { get; } = lspCompilationService;

    /// <returns>Obfuscated Script if successful, otherwise empty string</returns>
    public async Task<string> Handle( ExecuteCommandParams<string> request, CancellationToken cancellationToken )
    {
        var path = request.Arguments?.FirstOrDefault()?.ToString();

        if( string.IsNullOrWhiteSpace( path ) )
        {
            return string.Empty;
        }

        var uri = DocumentUri.Parse( path );

        var result = await LspCompilationService.ExecuteCompilationAsync(
            uri,
            new LspCompilationService.ExecuteCompileOption( enableObfuscation: true ),
            cancellationToken
        );

        return !result.Result
            ? string.Empty
            : result.ObfuscatedScript;
    }

    public ExecuteCommandRegistrationOptions GetRegistrationOptions( ExecuteCommandCapability capability, ClientCapabilities clientCapabilities )
    {
        return new ExecuteCommandRegistrationOptions
        {
            Commands = new Container<string>( ObfuscationCommand )
        };
    }
}
