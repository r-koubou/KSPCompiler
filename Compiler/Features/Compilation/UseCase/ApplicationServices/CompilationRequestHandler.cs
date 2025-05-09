using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.Compilation.Gateways.EventEmitting;
using KSPCompiler.Features.Compilation.Gateways.Parser;
using KSPCompiler.Features.Compilation.UseCase.Analysis;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions;
using KSPCompiler.Shared;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Blocks;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.EventEmitting;
using KSPCompiler.Shared.EventEmitting.Extensions;
using KSPCompiler.Shared.UseCase;

namespace KSPCompiler.Features.Compilation.UseCase.ApplicationServices;

public sealed class CompilationRequestHandler : ICompilationRequestHandler
{
    public async Task<CompilationResponse> HandleAsync( CompilationRequest request, CancellationToken cancellationToken = default )
    {
        var userSymbolTable = new AggregateSymbolTable();

        SetupSymbolTable( request.BuiltinSymbolTable, userSymbolTable );

        var noAnalysisError = true;

        try
        {
            var eventEmitter = request.EventEmitter;
            using var subscribers = new CompositeDisposable();

            eventEmitter.Subscribe<CompilationFatalEvent>( _ => noAnalysisError = false ).AddTo( subscribers );
            eventEmitter.Subscribe<CompilationErrorEvent>( _ => noAnalysisError = false ).AddTo( subscribers );

            //-------------------------------------------------
            // Syntax Analysis
            //-------------------------------------------------
            var syntaxAnalysisOutput = await ExecuteSyntaxAnalysisAsync( request.SyntaxParser, cancellationToken );
            var ast = syntaxAnalysisOutput.OutputData;

            //-------------------------------------------------
            // Preprocess
            //-------------------------------------------------
            var preprocessOutput = await ExecutePreprocessAsync( eventEmitter, ast, userSymbolTable, cancellationToken );

            if( !preprocessOutput.Result )
            {
                return new CompilationResponse( false, preprocessOutput.Error, ast, userSymbolTable, string.Empty );
            }

            //-------------------------------------------------
            // Semantic Analysis
            //-------------------------------------------------
            var semanticAnalysisOutput = await ExecuteSemanticAnalysisAsync( eventEmitter, ast, userSymbolTable, cancellationToken );

            if( !semanticAnalysisOutput.Result )
            {
                return new CompilationResponse( false, semanticAnalysisOutput.Error, ast, userSymbolTable, string.Empty );
            }

            //-------------------------------------------------
            // Obfuscation
            //-------------------------------------------------
            if( !request.EnableObfuscation || !noAnalysisError )
            {
                return new CompilationResponse( noAnalysisError, null, ast, userSymbolTable, string.Empty );
            }

            var obfuscationOutput = await ExecuteObfuscationAsync(
                eventEmitter,
                semanticAnalysisOutput.OutputData.CompilationUnitNode,
                semanticAnalysisOutput.OutputData.SymbolTable,
                cancellationToken
            );

            return new CompilationResponse(
                obfuscationOutput.Result,
                obfuscationOutput.Error,
                ast,
                userSymbolTable,
                obfuscationOutput.OutputData
            );
        }
        catch( Exception e )
        {
            return new CompilationResponse( false, e, null, userSymbolTable, string.Empty );
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
                new SemanticAnalysisOutputDataDetail(
                    ast,
                    symbolTable
                ), false, e
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
            return new ObfuscationOutputData( string.Empty, false, e );
        }
    }

    #region Setup Symbols
    private static void SetupSymbolTable( AggregateSymbolTable builtin, AggregateSymbolTable user )
    {
        user.Clear();
        AggregateSymbolTable.Merge( builtin, user );

        // ビルトイン変数は初期化済み扱い
        foreach( var variable in user.BuiltInVariables )
        {
            variable.State = SymbolState.Initialized;
        }
    }
    #endregion ~Setup Symbols
}
