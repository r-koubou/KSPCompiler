using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Controllers.Compiler;
using KSPCompiler.Domain.Events;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.Repositories;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Callbacks;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Commands;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.UITypes;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Variables;
using KSPCompiler.Infrastructures.Parser.Antlr;
using KSPCompiler.LSPServer.Core.Extensions;

namespace KSPCompiler.LSPServer.Core;

public class CompilerService
{
    private readonly AggregateSymbolTable builtInSymbolTable = new(
        builtInVariables: new VariableSymbolTable(),
        userVariables: new VariableSymbolTable(),
        uiTypes: new UITypeSymbolTable(),
        commands: new CommandSymbolTable(),
        builtInCallbacks: new CallbackSymbolTable(),
        userCallbacks: new CallbackSymbolTable(),
        userFunctions: new UserFunctionSymbolTable(),
        preProcessorSymbols: new PreProcessorSymbolTable()
    );

    private readonly CompilerController compilerController = new();

    public CompilerService()
    {
        // 外部定義ファイルからビルトイン変数、コマンド、コールバック、UIタイプを構築
        LoadSymbolTables( builtInSymbolTable );
    }

    public async Task<CompilerResult> CompileAsync( string script, IEventEmitter eventEmitter, CancellationToken cancellationToken )
    {
        SetupSymbol( builtInSymbolTable );

        var parser = new AntlrKspStringSyntaxParser( script, eventEmitter, Encoding.UTF8 );
        var symbolTableInScript = builtInSymbolTable.CreateBuiltInSymbolsOnly();

        var option = new CompilerOption(
            SyntaxParser: parser,
            SymbolTable: symbolTableInScript,
            SyntaxCheckOnly: false,
            EnableObfuscation: false
        );

        return await compilerController.ExecuteAsync( eventEmitter, option, cancellationToken );
    }

    private static void LoadSymbolTables( AggregateSymbolTable symbolTable )
    {
        var basePath = Path.Combine( "Data", "Symbols" );

        using ISymbolRepository<VariableSymbol> variables = new VariableSymbolRepository( Path.Combine( basePath, "variables.json" ) );
        symbolTable.BuiltInVariables.AddRange( variables.FindAll() );

        using ISymbolRepository<UITypeSymbol> uiTypes = new UITypeSymbolRepository( Path.Combine( basePath, "uitypes.json" ) );
        symbolTable.UITypes.AddRange( uiTypes.FindAll() );

        using ISymbolRepository<CommandSymbol> commands = new CommandSymbolRepository( Path.Combine( basePath, "commands.json" ) );
        symbolTable.Commands.AddRange( commands.FindAll() );

        using ISymbolRepository<CallbackSymbol> callbacks = new CallbackSymbolRepository( Path.Combine( basePath, "callbacks.json" ) );
        symbolTable.BuiltInCallbacks.AddRange( callbacks.FindAll() );
    }

    private static void SetupSymbol( AggregateSymbolTable symbolTable )
    {
        // 再突入時のため、ユーザー定義シンボルをクリア
        symbolTable.UserVariables.RemoveNoReservedSymbols();
        symbolTable.UserCallbacks.RemoveNoReservedSymbols();
        symbolTable.UserFunctions.RemoveNoReservedSymbols();

        // ビルトイン変数は初期化済み扱い
        foreach( var variable in symbolTable.BuiltInVariables )
        {
            variable.State = SymbolState.Initialized;
        }
    }
}
