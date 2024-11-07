using System;

using ConsoleAppFramework;

using KSPCompiler.Controllers.Compiler;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Infrastructures.Parser.Antlr;

// Run the compiler program.
ConsoleApp.Run( args, CompilerProgram.ExecuteCompiler );

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

        // TODO: 外部定義ファイルからビルトイン変数、コマンド、コールバック、UIタイプを構築

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
}
