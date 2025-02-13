using System;
using System.IO;
using System.Threading;

using KSPCompiler.Commons;
using KSPCompiler.Controllers.Compiler;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.CompilerMessages.Extensions;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.Yaml.Callbacks;
using KSPCompiler.ExternalSymbolRepository.Yaml.Commands;
using KSPCompiler.ExternalSymbolRepository.Yaml.UITypes;
using KSPCompiler.ExternalSymbolRepository.Yaml.Variables;
using KSPCompiler.Gateways.EventEmitting;
using KSPCompiler.Gateways.EventEmitting.Extensions;
using KSPCompiler.Infrastructures.EventEmitting.Default;
using KSPCompiler.Infrastructures.Parser.Antlr;

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

        var symbolTable = new AggregateSymbolTable(
            builtInVariables: new VariableSymbolTable(),
            userVariables: new VariableSymbolTable(),
            uiTypes: new UITypeSymbolTable(),
            commands: new CommandSymbolTable(),
            builtInCallbacks: new CallbackSymbolTable(),
            userCallbacks: new CallbackSymbolTable(),
            userFunctions: new UserFunctionSymbolTable(),
            preProcessorSymbols: new PreProcessorSymbolTable()
        );

        // 外部定義ファイルからビルトイン変数、コマンド、コールバック、UIタイプを構築
        LoadSymbolTables( symbolTable );

        // 追加のシンボルセットアップ処理
        SetupSymbolState( symbolTable );

        // イベントディスパッチャの設定
        SetupEventEmitter( eventEmitter, messageManager, subscribers );

        var parser = new AntlrKspFileSyntaxParser( input, eventEmitter );

        var compilerController = new CompilerController();
        var option = new CompilerOption(
            SyntaxParser: parser,
            SymbolTable: symbolTable,
            EnableObfuscation: enableObfuscation
        );

        var result = compilerController.Execute( eventEmitter, option );

        messageManager.WriteTo( Console.Out );

        if( result.Error is not null )
        {
            Console.WriteLine( result.Error );
        }

        return ( result.Error != null || !result.Result ) ? 1 : 0;
    }

    private static void LoadSymbolTables( AggregateSymbolTable symbolTable )
    {
        var basePath = Path.Combine( "Data", "Symbols" );

        using var variables = new VariableSymbolRepository( Path.Combine( basePath, "variables.yaml" ) );
        symbolTable.BuiltInVariables.AddRange( variables.FindAllAsync( CancellationToken.None ).GetAwaiter().GetResult() );

        using var uiTypes = new UITypeSymbolRepository( Path.Combine( basePath, "uitypes.yaml" ) );
        symbolTable.UITypes.AddRange( uiTypes.FindAllAsync( CancellationToken.None ).GetAwaiter().GetResult() );

        using var commands = new CommandSymbolRepository( Path.Combine( basePath, "commands.yaml" ) );
        symbolTable.Commands.AddRange( commands.FindAllAsync( CancellationToken.None ).GetAwaiter().GetResult() );

        using var callbacks = new CallbackSymbolRepository( Path.Combine( basePath, "callbacks.yaml" ) );
        var foundCallbacks = callbacks.FindAllAsync( CancellationToken.None ).GetAwaiter().GetResult();

        foreach( var x in foundCallbacks )
        {
            if( x.AllowMultipleDeclaration )
            {
                symbolTable.BuiltInCallbacks.AddAsOverload( x, x.Arguments );
            }
            else
            {
                symbolTable.BuiltInCallbacks.AddAsNoOverload( x );
            }
        }
    }

    private static void SetupSymbolState( AggregateSymbolTable symbolTable )
    {
        // ビルトイン変数は初期化済み扱い
        foreach( var variable in symbolTable.BuiltInVariables )
        {
            variable.State = SymbolState.Initialized;
        }
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
