using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Events;
using KSPCompiler.Domain.Events.Extensions;
using KSPCompiler.Interactors.Analysis.Semantics;
using KSPCompiler.UseCases.Analysis;

namespace KSPCompiler.Interactors.Analysis;

public class SemanticAnalysisInteractor : ISemanticAnalysisUseCase
{
    public Task<SemanticAnalysisOutputData> ExecuteAsync( SemanticAnalysisInputData parameter, CancellationToken cancellationToken = default )
    {
        var node = parameter.InputData.CompilationUnitNode;
        var symbolTable = parameter.InputData.SymbolTable;

        var noError = true;
        var eventEmitter = parameter.InputData.EventEmitter;

        try
        {
            using var subscribers = new CompositeDisposable();
            eventEmitter.Subscribe<CompilationFatalEvent>( _ => noError = false ).AddTo( subscribers );
            eventEmitter.Subscribe<CompilationErrorEvent>( _ => noError = false ).AddTo( subscribers );

            var context = new SemanticAnalyzerContext( eventEmitter, symbolTable );
            var analyzer = new SemanticAnalyzer( context );

            analyzer.Traverse( parameter.InputData.CompilationUnitNode );
        }
        catch( Exception e )
        {
            return Task.FromResult( CreateOutputData( false, e ) );
        }

        return Task.FromResult( CreateOutputData( noError, null ) );

        SemanticAnalysisOutputData CreateOutputData( bool result, Exception? error )
            => new( result, error, new SemanticAnalysisOutputDataDetail( node, symbolTable ) );
    }

    public SemanticAnalysisOutputData Execute( SemanticAnalysisInputData input )
        => ExecuteAsync( input ).GetAwaiter().GetResult();
}
