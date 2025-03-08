using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using EmmyLua.LanguageServer.Framework.Protocol.Message.Client.PublishDiagnostics;
using EmmyLua.LanguageServer.Framework.Protocol.Model.Diagnostic;
using EmmyLua.LanguageServer.Framework.Server;

using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Compilation.Extensions;
using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Extensions;
using KSPCompiler.Features.Compilation.Domain;
using KSPCompiler.Features.Compilation.Gateways.EventEmitting;
using KSPCompiler.Features.Compilation.Infrastructures.Parser.Antlr;
using KSPCompiler.Features.Compilation.UseCase.ApplicationServices;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions;
using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Compilation;
using KSPCompiler.Shared;
using KSPCompiler.Shared.Domain.Compilation;
using KSPCompiler.Shared.EventEmitting;
using KSPCompiler.Shared.EventEmitting.Extensions;

namespace KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Compilation;

public sealed class CompilationServerService(
    ClientProxy client,
    CompilationApplicationService applicationService
)
{
    private readonly ClientProxy client = client;
    private readonly CompilationApplicationService applicationService = applicationService;

    public async Task<CompilationResult> CompileAsync(
        ICompilationCacheManager compilationCacheManager,
        ScriptLocation scriptLocation,
        string script,
        bool enableObfuscation = false,
        CancellationToken cancellationToken = default )
    {
        var diagnostics = new List<Diagnostic>();
        using var eventEmitter = new EventEmitter();

        using var compilerEventSubscribers = new CompositeDisposable();

        #region Subscription compilation events
        eventEmitter.Subscribe<CompilationErrorEvent>(
            e =>
            {
                diagnostics.Add( e.AsDiagnostic() );
            }
        ).AddTo( compilerEventSubscribers );

        eventEmitter.Subscribe<CompilationWarningEvent>( e =>
            {
                diagnostics.Add( e.AsDiagnostic() );
            }
        ).AddTo( compilerEventSubscribers );
        #endregion ~Event Subscription

        #region Compilation
        var compileOption = new CompilationOption(
            new AntlrKspStringSyntaxParser( script, eventEmitter, Encoding.UTF8 ),
            enableObfuscation
        );

        var compilationResult = await applicationService.ExecuteAsync(
            eventEmitter,
            compileOption,
            cancellationToken
        );
        #endregion ~Compilation

        #region Publish Diagnostics
        await client.PublishDiagnostics( new PublishDiagnosticsParams
            {
                Uri         = scriptLocation.AsDocumentUri(),
                Diagnostics = diagnostics
            }
        );
        #endregion ~Publish Diagnostics

        var allLinesText = GetScriptLines( script );

        compilationCacheManager.UpdateCache(
            scriptLocation,
            new CompilationCacheItem(
                scriptLocation: scriptLocation,
                allLinesText: allLinesText,
                symbolTable: compilationResult.SymbolTable,
                ast: compilationResult.Ast
            )
        );

        return compilationResult;
    }

    public async Task ClearDiagnosticAsync( ScriptLocation scriptLocation, CancellationToken _ )
    {
        await client.PublishDiagnostics( new PublishDiagnosticsParams
            {
                Uri         = scriptLocation.AsDocumentUri(),
                Diagnostics = []
            }
        );

        await Task.CompletedTask;
    }

    private static List<string> GetScriptLines( string script )
    {
        var result = new List<string>( 256 );
        using var reader = new StringReader( script );

        while( reader.ReadLine() is {} line )
        {
            result.Add( line );
        }

        return result;
    }
}
