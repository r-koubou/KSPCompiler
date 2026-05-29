using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.Compilation.Gateways.EventEmitting;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Evaluations;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics;
using KSPCompiler.Shared;
using KSPCompiler.Shared.EventEmitting.Extensions;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis;

public class SemanticAnalysisInteractor : ISemanticAnalysisUseCase
{
    public async Task<Result<SemanticAnalysisOutput, CompilationFailureReason>> ExecuteAsync( SemanticAnalysisInput input, CancellationToken cancellationToken = default )
    {
        var node = input.CompilationUnitNode;
        var symbolTable = input.SymbolTable;

        var noError = true;
        var eventEmitter = input.EventEmitter;

        try
        {
            using var subscribers = new CompositeDisposable();
            eventEmitter.Subscribe<CompilationFatalEvent>( _ => noError = false ).AddTo( subscribers );
            eventEmitter.Subscribe<CompilationErrorEvent>( _ => noError = false ).AddTo( subscribers );

            var context = new SemanticAnalyzerContext( eventEmitter, symbolTable );
            var analyzer = new SemanticAnalyzer( context );

            analyzer.Traverse( input.CompilationUnitNode );

            await Task.CompletedTask;

            return noError
                ? Result<SemanticAnalysisOutput, CompilationFailureReason>.Success( new SemanticAnalysisOutput( node, symbolTable ) )
                : Result<SemanticAnalysisOutput, CompilationFailureReason>.Failure( CompilationFailureReason.SemanticsError );
        }
        catch( AstAnalyzeException e )
        {
            return Result<SemanticAnalysisOutput, CompilationFailureReason>.Failure( CompilationFailureReason.SemanticsError, e );
        }
        catch( Exception e )
        {
            return Result<SemanticAnalysisOutput, CompilationFailureReason>.Failure( CompilationFailureReason.Other, e );
        }
    }
}
