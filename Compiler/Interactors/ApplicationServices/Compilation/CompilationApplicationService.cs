using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Gateways.EventEmitting;
using KSPCompiler.Gateways.EventEmitting.Extensions;
using KSPCompiler.Gateways.Parsers;
using KSPCompiler.Gateways.Symbols;
using KSPCompiler.Interactors.Analysis;
using KSPCompiler.UseCases;
using KSPCompiler.UseCases.Analysis;
using KSPCompiler.UseCases.Symbols;

namespace KSPCompiler.Interactors.ApplicationServices.Compilation;

public sealed record CompilationOption(
    ISyntaxParser SyntaxParser,
    bool EnableObfuscation );

public sealed record CompilationResult(
    bool Result,
    Exception? Error,
    AstCompilationUnitNode? Ast,
    AggregateSymbolTable SymbolTable,
    string ObfuscatedScript );

public sealed class CompilationApplicationService(
    ILoadBuiltinSymbolUseCase loadBuiltinSymbolUseCase,
    AggregateSymbolRepository symbolRepositories
)
{
    private readonly ILoadBuiltinSymbolUseCase loadBuiltinSymbolUseCase = loadBuiltinSymbolUseCase;
    private readonly AggregateSymbolRepository symbolRepositories = symbolRepositories;
    private AggregateSymbolTable? builtInSymbolTable;

    public CompilationResult Execute( IEventEmitter eventEmitter, CompilationOption option )
        => ExecuteAsync( eventEmitter, option, CancellationToken.None ).GetAwaiter().GetResult();

    // ReSharper disable once UnusedMethodReturnValue.Global
    public async Task<CompilationResult> ExecuteAsync( IEventEmitter eventEmitter, CompilationOption option, CancellationToken cancellationToken )
    {
        // TODO: CompilerMessageManger compilerMessageManger is obsolete. Use IEventEmitter eventEmitter instead.

        if( builtInSymbolTable == null )
        {
            await LoadBuiltInSymbolsAsync( cancellationToken );
        }

        if( builtInSymbolTable == null )
        {
            throw new InvalidOperationException( "Failed to load built-in symbols." );
        }

        var userSymbolTable = AggregateSymbolTable.Default();

        SetupSymbolTable( builtInSymbolTable, userSymbolTable );

        var noAnalysisError = true;

        try
        {
            using var subscribers = new CompositeDisposable();
            eventEmitter.Subscribe<CompilationFatalEvent>( _ => noAnalysisError = false ).AddTo( subscribers );
            eventEmitter.Subscribe<CompilationErrorEvent>( _ => noAnalysisError = false ).AddTo( subscribers );

            //-------------------------------------------------
            // Syntax Analysis
            //-------------------------------------------------
            var syntaxAnalysisOutput = await ExecuteSyntaxAnalysisAsync( option.SyntaxParser, cancellationToken );
            var ast = syntaxAnalysisOutput.OutputData;

            //-------------------------------------------------
            // Preprocess
            //-------------------------------------------------
            var preprocessOutput = await ExecutePreprocessAsync( eventEmitter, ast, userSymbolTable, cancellationToken );

            if( !preprocessOutput.Result )
            {
                return new CompilationResult( false, preprocessOutput.Error, ast, userSymbolTable, string.Empty );
            }

            //-------------------------------------------------
            // Semantic Analysis
            //-------------------------------------------------
            var semanticAnalysisOutput = await ExecuteSemanticAnalysisAsync( eventEmitter, ast, userSymbolTable, cancellationToken );

            if( !semanticAnalysisOutput.Result )
            {
                return new CompilationResult( false, semanticAnalysisOutput.Error, ast, userSymbolTable, string.Empty );
            }

            //-------------------------------------------------
            // Obfuscation
            //-------------------------------------------------
            if( !option.EnableObfuscation || !noAnalysisError )
            {
                return new CompilationResult( noAnalysisError, null, ast, userSymbolTable, string.Empty );
            }

            var obfuscationOutput = await ExecuteObfuscationAsync(
                eventEmitter,
                semanticAnalysisOutput.OutputData.CompilationUnitNode,
                semanticAnalysisOutput.OutputData.SymbolTable,
                cancellationToken
            );

            return new CompilationResult(
                obfuscationOutput.Result,
                obfuscationOutput.Error,
                ast,
                userSymbolTable,
                obfuscationOutput.OutputData
            );
        }
        catch( Exception e )
        {
            return new CompilationResult( false, e, null, userSymbolTable, string.Empty );
        }
    }

    private async Task<SyntaxAnalysisOutputData> ExecuteSyntaxAnalysisAsync( ISyntaxParser parser, CancellationToken cancellationToken )
    {
        var analyzer = new SyntaxAnalysisInteractor();
        var input = new SyntaxAnalysisInputData( parser );

        return await analyzer.ExecuteAsync( input, cancellationToken );
    }

    private async Task<UnitOutputPort> ExecutePreprocessAsync(
        IEventEmitter compilerMessageManger,
        AstCompilationUnitNode ast,
        AggregateSymbolTable symbolTable,
        CancellationToken cancellationToken )
    {
        IPreprocessUseCase preprocessor = new PreprocessInteractor();
        var preprocessInput = new PreprocessInputData(
            new PreprocessInputDataDetail( compilerMessageManger, ast, symbolTable )
        );

        return await preprocessor.ExecuteAsync( preprocessInput, cancellationToken );
    }

    private async Task<SemanticAnalysisOutputData> ExecuteSemanticAnalysisAsync(
        IEventEmitter eventEmitter,
        AstCompilationUnitNode ast,
        AggregateSymbolTable symbolTable,
        CancellationToken cancellationToken )
    {
        var semanticAnalyzer = new SemanticAnalysisInteractor();
        var preprocessInput = new SemanticAnalysisInputData(
            new SemanticAnalysisInputDataDetail( eventEmitter, ast, symbolTable )
        );

        try
        {
            return await semanticAnalyzer.ExecuteAsync( preprocessInput, cancellationToken );
        }
        catch( Exception e )
        {
            return new SemanticAnalysisOutputData(
                false,
                e,
                new SemanticAnalysisOutputDataDetail(
                    ast,
                    symbolTable
                )
            );
        }
    }

    private async Task<ObfuscationOutputData> ExecuteObfuscationAsync(
        IEventEmitter eventEmitter,
        AstCompilationUnitNode ast,
        AggregateSymbolTable symbolTable,
        CancellationToken cancellationToken )
    {
        var obfuscator = new ObfuscationInteractor();
        var input = new ObfuscationInputData(
            new ObfuscationInputDataDetail( eventEmitter, ast, symbolTable )
        );

        try
        {
            return await obfuscator.ExecuteAsync( input, cancellationToken );
        }
        catch( Exception e )
        {
            return new ObfuscationOutputData( false, e, string.Empty );
        }
    }

    #region Setup Symbols
    private async Task LoadBuiltInSymbolsAsync( CancellationToken cancellationToken )
    {
        if( builtInSymbolTable is not null )
        {
            return;
        }

        var input = new LoadBuiltinSymbolInputData( symbolRepositories );
        var result = await loadBuiltinSymbolUseCase.ExecuteAsync( input, cancellationToken );

        if( !result.Result )
        {
            throw new InvalidOperationException( "Failed to load built-in symbols.", result.Error );
        }

        builtInSymbolTable = result.OutputData;
        symbolRepositories.Dispose();
    }

    private static void SetupSymbolTable( AggregateSymbolTable builtin, AggregateSymbolTable user )
    {
        AggregateSymbolTable.Merge( builtin, user, clearTarget: true );

        // ビルトイン変数は初期化済み扱い
        foreach( var variable in user.BuiltInVariables )
        {
            variable.State = SymbolState.Initialized;
        }
    }
    #endregion ~Setup Symbols
}
