using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons;
using KSPCompiler.Gateways.EventEmitting;
using KSPCompiler.Gateways.EventEmitting.Extensions;
using KSPCompiler.Interactors.Analysis.Semantics;
using KSPCompiler.UseCases.Analysis;

namespace KSPCompiler.Interactors.Analysis;

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
