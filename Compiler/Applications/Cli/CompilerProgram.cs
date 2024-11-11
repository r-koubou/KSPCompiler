using System;
using System.IO;
using System.Threading;

using KSPCompiler.Controllers.Compiler;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Callbacks;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Commands;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.UITypes;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Variables;
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
    /// <param name="syntaxCheckOnly">Syntax check only. (No semantic analysis)</param>
    /// <param name="enableObfuscation">Run obfuscation after compiling.</param>
    public static int ExecuteCompiler(
        string input,
        bool syntaxCheckOnly = false,
        bool enableObfuscation = false )
    {
        var messageManager = ICompilerMessageManger.Default;
        var symbolTable = new AggregateSymbolTable(
            new VariableSymbolTable(),
            new UITypeSymbolTable(),
            new CommandSymbolTable(),
            new CallbackSymbolTable(),
            new CallbackSymbolTable(),
            new UserFunctionSymbolTable(),
            new PreProcessorSymbolTable()
        );

        // 外部定義ファイルからビルトイン変数、コマンド、コールバック、UIタイプを構築
        LoadSymbolTables( symbolTable );

        // 追加のシンボルセットアップ処理
        SetupSymbolState( symbolTable );

        var parser = new AntlrKspFileSyntaxParser( input, messageManager );

        var compilerController = new CompilerController();
        var option = new CompilerOption(
            SyntaxParser: parser,
            symbolTable: symbolTable,
            SyntaxCheckOnly: syntaxCheckOnly,
            EnableObfuscation: enableObfuscation
        );

        compilerController.Execute( messageManager, option );

        messageManager.WriteTo( Console.Out );

        return messageManager.Count( CompilerMessageLevel.Error ) > 0 ? 1 : 0;
    }

    private static void LoadSymbolTables( AggregateSymbolTable symbolTable )
    {
        var basePath = Path.Combine( "Data", "Symbols" );

        using var variables = new VariableSymbolRepository( Path.Combine( basePath, "variables.json" ) );
        symbolTable.Variables.AddRange( variables.FindAllAsync( CancellationToken.None ).GetAwaiter().GetResult() );

        using var uiTypes = new UITypeSymbolRepository( Path.Combine( basePath, "uitypes.json" ) );
        symbolTable.UITypes.AddRange( uiTypes.FindAllAsync( CancellationToken.None ).GetAwaiter().GetResult() );

        using var commands = new CommandSymbolRepository( Path.Combine( basePath, "commands.json" ) );
        symbolTable.Commands.AddRange( commands.FindAllAsync( CancellationToken.None ).GetAwaiter().GetResult() );

        using var callbacks = new CallbackSymbolRepository( Path.Combine( basePath, "callbacks.json" ) );
        symbolTable.ReservedCallbacks.AddRange( callbacks.FindAllAsync( CancellationToken.None ).GetAwaiter().GetResult() );
    }

    private static void SetupSymbolState( AggregateSymbolTable symbolTable )
    {
        // ビルトイン変数は初期化済み扱い
        foreach( var variable in symbolTable.Variables )
        {
            variable.State = SymbolState.Initialized;
        }
    }
}
