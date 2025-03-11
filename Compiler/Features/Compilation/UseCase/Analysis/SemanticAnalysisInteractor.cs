using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.Compilation.Gateways.EventEmitting;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics;
using KSPCompiler.Shared;
using KSPCompiler.Shared.EventEmitting.Extensions;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis;

public class SemanticAnalysisInteractor : ISemanticAnalysisUseCase
{
    public Task<SemanticAnalysisOutputData> ExecuteAsync( SemanticAnalysisInputData parameter, CancellationToken cancellationToken = default )
    {
        var node = parameter.Input.CompilationUnitNode;
        var symbolTable = parameter.Input.SymbolTable;

        var noError = true;
        var eventEmitter = parameter.Input.EventEmitter;

        try
        {
            using var subscribers = new CompositeDisposable();
            eventEmitter.Subscribe<CompilationFatalEvent>( _ => noError = false ).AddTo( subscribers );
            eventEmitter.Subscribe<CompilationErrorEvent>( _ => noError = false ).AddTo( subscribers );

            var context = new SemanticAnalyzerContext( eventEmitter, symbolTable );
            var analyzer = new SemanticAnalyzer( context );

            analyzer.Traverse( parameter.Input.CompilationUnitNode );
        }
        catch( Exception e )
        {
            return Task.FromResult( CreateOutputData( false, e ) );
        }

        return Task.FromResult( CreateOutputData( noError, null ) );

        SemanticAnalysisOutputData CreateOutputData( bool result, Exception? error )
            => new( new SemanticAnalysisOutputDataDetail( node, symbolTable ), result, error );
    }

    public SemanticAnalysisOutputData Execute( SemanticAnalysisInputData input )
        => ExecuteAsync( input ).GetAwaiter().GetResult();
}
