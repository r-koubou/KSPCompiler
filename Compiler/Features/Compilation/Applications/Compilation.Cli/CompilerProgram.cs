using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.Compilation.Domain.Messages;
using KSPCompiler.Features.Compilation.Domain.Messages.Extensions;
using KSPCompiler.Features.Compilation.Gateways.EventEmitting;
using KSPCompiler.Features.Compilation.Gateways.Symbol;
using KSPCompiler.Features.Compilation.Infrastructures.BuiltInSymbolLoader.Yaml;
using KSPCompiler.Features.Compilation.Infrastructures.Parser.Antlr;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions;
using KSPCompiler.Features.Compilation.UseCase.ApplicationServices;
using KSPCompiler.Shared;
using KSPCompiler.Shared.EventEmitting;
using KSPCompiler.Shared.EventEmitting.Extensions;

namespace KSPCompiler.Features.Compilation.Applications.Compilation.Cli;

/// <summary>
/// Body of the compiler program.
/// </summary>
public static class CompilerProgram
{
    /// <summary>
    /// A compiler program for KSP (Kontakt Script Processor)
    /// </summary>
    /// <param name="input">A script file path to compile.</param>
    /// <param name="enableObfuscation">Run obfuscation after compiling.</param>
    /// <param name="cancellationToken"></param>
    public static async Task<int> ExecuteCompilerAsync(
        string input,
        bool enableObfuscation = false,
        CancellationToken cancellationToken = default )
    {
        using var subscribers = new CompositeDisposable();
        var eventEmitter = new EventEmitter();
        var messageManager = ICompilerMessageManger.Default;

        // イベントディスパッチャの設定
        SetupEventEmitter( eventEmitter, messageManager, subscribers );

        var parser = new AntlrKspFileSyntaxParser( input, eventEmitter );
        var builtinSymbolLoader = CreateBuiltinSymbolLoader();
        var builtinSymbolTable = await builtinSymbolLoader.LoadAsync( cancellationToken );
        var compilationRequestHandler = new CompilationRequestHandler();
        ICompilationMediator compilationMediator = new CompilationMediator( compilationRequestHandler );

        var request = new CompilationRequest(
            SyntaxParser: parser,
            BuiltinSymbolTable: builtinSymbolTable,
            EventEmitter: eventEmitter,
            EnableObfuscation: enableObfuscation
        );

        var response = await compilationMediator.RequestAsync( request, cancellationToken );

        messageManager.WriteTo( Console.Out );

        if( response.Error is not null )
        {
            Console.WriteLine( response.Error );
        }

        Console.WriteLine(response.ObfuscatedScript);

        return ( response.Error != null || !response.Result ) ? 1 : 0;
    }

    private static IBuiltInSymbolLoader CreateBuiltinSymbolLoader()
    {
        var baseDir = Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location ) ?? ".";
        var basePath = Path.Combine( baseDir, "Data", "Symbols" );
        return new YamlBuiltInSymbolLoader( basePath );
    }

    private static void SetupEventEmitter( EventEmitter eventEmitter, ICompilerMessageManger messageManager, CompositeDisposable subscribers )
    {
        eventEmitter.Subscribe<CompilationFatalEvent>(
            evt => messageManager.Fatal( evt.Position, evt.Message )
        ).AddTo( subscribers );

        eventEmitter.Subscribe<CompilationErrorEvent>(
            evt => messageManager.Error( evt.Position, evt.Message )
        ).AddTo( subscribers );

        eventEmitter.Subscribe<CompilationWarningEvent>(
            evt => messageManager.Warning( evt.Position, evt.Message )
        ).AddTo( subscribers );
    }
}
