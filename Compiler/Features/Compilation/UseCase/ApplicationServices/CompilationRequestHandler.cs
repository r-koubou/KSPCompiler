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

            if( !syntaxAnalysisOutput.Result )
            {
                return new CompilationResponse( false, syntaxAnalysisOutput.Error, null, userSymbolTable, string.Empty );
            }

            var ast = syntaxAnalysisOutput.OutputData;

            //-------------------------------------------------
            // Preprocess
            //-------------------------------------------------
            var preprocessResult = await ExecutePreprocessAsync( eventEmitter, ast, userSymbolTable, cancellationToken );

            if( preprocessResult.IsFailure )
            {
                return new CompilationResponse( false, preprocessResult.UnwrapError().Error, ast, userSymbolTable, string.Empty );
            }

            //-------------------------------------------------
            // Semantic Analysis
            //-------------------------------------------------
            var semanticAnalysisResult = await ExecuteSemanticAnalysisAsync( eventEmitter, ast, userSymbolTable, cancellationToken );

            if( semanticAnalysisResult.IsFailure )
            {
                return new CompilationResponse( false, semanticAnalysisResult.UnwrapError().Error, ast, userSymbolTable, string.Empty );
            }

            var semanticAnalysisOutput = semanticAnalysisResult.Unwrap();

            //-------------------------------------------------
            // Obfuscation
            //-------------------------------------------------
            if( !request.EnableObfuscation || !noAnalysisError )
            {
                return new CompilationResponse( noAnalysisError, null, ast, userSymbolTable, string.Empty );
            }

            var obfuscateResult = await ExecuteObfuscationAsync(
                eventEmitter,
                semanticAnalysisOutput.CompilationUnitNode,
                semanticAnalysisOutput.SymbolTable,
                cancellationToken
            );

            return obfuscateResult.IsSuccess
                ? new CompilationResponse( true, null, ast, userSymbolTable, obfuscateResult.Unwrap().ObfuscatedScript )
                : new CompilationResponse( false, obfuscateResult.UnwrapError().Error, ast, userSymbolTable, string.Empty );
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

    private async Task<Result<Unit, CompilationFailureReason>> ExecutePreprocessAsync(
        IEventEmitter compilerMessageManger,
        AstCompilationUnitNode ast,
        AggregateSymbolTable symbolTable,
        CancellationToken cancellationToken )
    {
        IPreprocessUseCase preprocessor = new PreprocessInteractor();
        var preprocessInput = new PreprocessInput( compilerMessageManger, ast, symbolTable );

        return await preprocessor.ExecuteAsync( preprocessInput, cancellationToken );
    }

    private static async Task<Result<SemanticAnalysisOutput, CompilationFailureReason>> ExecuteSemanticAnalysisAsync(
        IEventEmitter eventEmitter,
        AstCompilationUnitNode ast,
        AggregateSymbolTable symbolTable,
        CancellationToken cancellationToken )
    {
        var semanticAnalyzer = new SemanticAnalysisInteractor();
        var preprocessInput = new SemanticAnalysisInput( eventEmitter, ast, symbolTable );

        return await semanticAnalyzer.ExecuteAsync( preprocessInput, cancellationToken );
    }

    private static async Task<Result<ObfuscationOutput, CompilationFailureReason>> ExecuteObfuscationAsync(
        IEventEmitter eventEmitter,
        AstCompilationUnitNode ast,
        AggregateSymbolTable symbolTable,
        CancellationToken cancellationToken )
    {
        var obfuscator = new ObfuscationInteractor();
        var input = new ObfuscationInput( eventEmitter, ast, symbolTable );

        return await obfuscator.ExecuteAsync( input, cancellationToken );
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
