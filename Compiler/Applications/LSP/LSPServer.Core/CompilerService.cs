using System.IO;
using System.Text;
using System.Threading;

using KSPCompiler.Controllers.Compiler;
using KSPCompiler.Domain.Events;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Callbacks;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Commands;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.UITypes;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Variables;
using KSPCompiler.Infrastructures.Parser.Antlr;

namespace KSPCompiler.LSPServer.Core;

public class CompilerService
{
    private readonly AggregateSymbolTable symbolTable = new AggregateSymbolTable(
        new VariableSymbolTable(),
        new UITypeSymbolTable(),
        new CommandSymbolTable(),
        new CallbackSymbolTable(),
        new CallbackSymbolTable(),
        new UserFunctionSymbolTable(),
        new PreProcessorSymbolTable()
    );

    private readonly CompilerController compilerController = new CompilerController();

    public CompilerService()
    {
        // 外部定義ファイルからビルトイン変数、コマンド、コールバック、UIタイプを構築
        LoadSymbolTables( symbolTable );
    }

    public void Compile( string script, IEventEmitter eventEmitter )
    {
        // 追加のシンボルセットアップ処理
        SetupSymbolState( symbolTable );

        var parser = new AntlrKspStringSyntaxParser( script, eventEmitter, Encoding.UTF8 );
        var option = new CompilerOption(
            SyntaxParser: parser,
            SymbolTable: symbolTable,
            SyntaxCheckOnly: false,
            EnableObfuscation: false
        );

        compilerController.Execute( eventEmitter, option );
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
        symbolTable.BuiltInCallbacks.AddRange( callbacks.FindAllAsync( CancellationToken.None ).GetAwaiter().GetResult() );
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
