using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Interactor.Analysis.Semantics;
using KSPCompiler.UseCases.Analysis;

namespace KSPCompiler.Interactor.Analysis;

public class SemanticAnalysisInteractor : ISemanticAnalysisUseCase
{
    public Task<SemanticAnalysisOutputData> ExecuteAsync( SemanticAnalysisInputData parameter, CancellationToken cancellationToken = default )
    {
        var messageManger = parameter.InputData.MessageManager;
        var node = parameter.InputData.CompilationUnitNode;
        var symbolTable = parameter.InputData.SymbolTable;

        var context = new SemanticAnalyzerContext( messageManger, symbolTable );
        var preprocessor = new SemanticAnalyzer( context );

        try
        {
            preprocessor.Traverse( parameter.InputData.CompilationUnitNode );
        }
        catch( Exception e )
        {
            return Task.FromResult( CreateOutputData( false, e ) );
        }

        return Task.FromResult( CreateOutputData( true, null ) );

        SemanticAnalysisOutputData CreateOutputData( bool result, Exception? error )
            => new( result, error, new SemanticAnalysisOutputDataDetail( messageManger, node, symbolTable ) );
    }
}
