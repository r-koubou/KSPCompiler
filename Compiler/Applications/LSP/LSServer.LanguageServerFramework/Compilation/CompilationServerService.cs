using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using EmmyLua.LanguageServer.Framework.Protocol.Message.Client.PublishDiagnostics;
using EmmyLua.LanguageServer.Framework.Protocol.Model.Diagnostic;
using EmmyLua.LanguageServer.Framework.Server;

using KSPCompiler.Applications.LSPServer.CoreNew;
using KSPCompiler.Applications.LSPServer.CoreNew.Compilation;
using KSPCompiler.Applications.LSServer.LanguageServerFramework.Compilation.Extensions;
using KSPCompiler.Applications.LSServer.LanguageServerFramework.Extensions;
using KSPCompiler.Commons;
using KSPCompiler.Gateways.EventEmitting;
using KSPCompiler.Gateways.EventEmitting.Extensions;
using KSPCompiler.Infrastructures.EventEmitting.Default;
using KSPCompiler.Infrastructures.Parser.Antlr;
using KSPCompiler.Interactors.ApplicationServices.Compilation;

namespace KSPCompiler.Applications.LSServer.LanguageServerFramework.Compilation;

public sealed class CompilationServerService(
    ClientProxy client,
    CompilationApplicationService applicationService
)
{
    private readonly ClientProxy client = client;
    private readonly CompilationApplicationService applicationService = applicationService;

    public async Task CompileAsync(
        CompilationCacheManager compilationCacheManager,
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
