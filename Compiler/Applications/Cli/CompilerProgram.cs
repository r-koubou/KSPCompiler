using System;
using System.IO;
using System.Reflection;

using KSPCompiler.Commons;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.CompilerMessages.Extensions;
using KSPCompiler.ExternalSymbolRepository.Yaml.Callbacks;
using KSPCompiler.ExternalSymbolRepository.Yaml.Commands;
using KSPCompiler.ExternalSymbolRepository.Yaml.UITypes;
using KSPCompiler.ExternalSymbolRepository.Yaml.Variables;
using KSPCompiler.Gateways.EventEmitting;
using KSPCompiler.Gateways.EventEmitting.Extensions;
using KSPCompiler.Gateways.Symbols;
using KSPCompiler.Infrastructures.EventEmitting.Default;
using KSPCompiler.Infrastructures.Parser.Antlr;
using KSPCompiler.Interactors.ApplicationServices.Compilation;
using KSPCompiler.Interactors.Symbols;

namespace KSPCompiler.Applications.Cli;

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
    public static int ExecuteCompiler(
        string input,
        bool enableObfuscation = false )
    {
        using var subscribers = new CompositeDisposable();
        var eventEmitter = new EventEmitter();
        var messageManager = ICompilerMessageManger.Default;

        // イベントディスパッチャの設定
        SetupEventEmitter( eventEmitter, messageManager, subscribers );

        var parser = new AntlrKspFileSyntaxParser( input, eventEmitter );

        var compilationService = new CompilationApplicationService(
            new LoadBuiltinSymbolInteractor(),
            CreateSymbolRepositories()
        );

        var option = new CompilationOption(
            SyntaxParser: parser,
            EnableObfuscation: enableObfuscation
        );

        var result = compilationService.Execute( eventEmitter, option );

        messageManager.WriteTo( Console.Out );

        if( result.Error is not null )
        {
            Console.WriteLine( result.Error );
        }

        return ( result.Error != null || !result.Result ) ? 1 : 0;
    }

    private static AggregateSymbolRepository CreateSymbolRepositories()
    {
        var baseDir = Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location ) ?? ".";
        var basePath = Path.Combine( baseDir, "Data", "Symbols" );

        return new AggregateSymbolRepository(
            new VariableSymbolRepository( Path.Combine( basePath, "variables.yaml" ) ),
            new UITypeSymbolRepository( Path.Combine( basePath, "uitypes.yaml" ) ),
            new CommandSymbolRepository( Path.Combine( basePath, "commands.yaml" ) ),
            new CallbackSymbolRepository( Path.Combine( basePath, "callbacks.yaml" )
            )
        );
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
