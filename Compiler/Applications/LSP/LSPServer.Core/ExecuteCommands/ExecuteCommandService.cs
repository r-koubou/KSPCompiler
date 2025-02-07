using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.LSPServer.Core.Compilations;

using MediatR;

using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.LSPServer.Core.ExecuteCommands;

public sealed class ExecuteCommandService( CompilationService compilationService )
{
    private const string ObfuscationCommand = "kspcompiler.obfuscate";

    private CompilationService CompilationService { get; } = compilationService;

    public async Task<object> HandleAsync( ExecuteCommandParams<object> request, CancellationToken cancellationToken )
    {
        if( request.Command == ObfuscationCommand )
        {
            return await HandleObfuscationCommandAsync( request, cancellationToken );
        }

        return Unit.Value;
    }

    /// <summary>
    /// Implementation of Obfuscation Command
    /// </summary>
    /// <remarks>
    /// <para>request.Arguments</para>
    /// <list type="number">
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
    /// <returns>Obfuscated Script if successful, otherwise empty string</returns>
    private async Task<object> HandleObfuscationCommandAsync( ExecuteCommandParams<object> request, CancellationToken cancellationToken )
    {
        var path = request.Arguments?.FirstOrDefault()?.ToString();

        if( string.IsNullOrWhiteSpace( path ) )
        {
            return string.Empty;
        }

        var uri = DocumentUri.Parse( path );

        var result = await CompilationService.ExecuteCompilationAsync(
            uri,
            new CompilationService.ExecuteCompileOption( enableObfuscation: true ),
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
