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
        var node = parameter.Data.CompilationUnitNode;
        var symbolTable = parameter.Data.SymbolTable;

        var noError = true;
        var eventEmitter = parameter.Data.EventEmitter;

        try
        {
            using var subscribers = new CompositeDisposable();
            eventEmitter.Subscribe<CompilationFatalEvent>( _ => noError = false ).AddTo( subscribers );
            eventEmitter.Subscribe<CompilationErrorEvent>( _ => noError = false ).AddTo( subscribers );

            var context = new SemanticAnalyzerContext( eventEmitter, symbolTable );
            var analyzer = new SemanticAnalyzer( context );

            analyzer.Traverse( parameter.Data.CompilationUnitNode );
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
