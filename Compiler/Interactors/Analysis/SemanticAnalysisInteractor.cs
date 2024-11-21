using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Interactors.Analysis.Semantics;
using KSPCompiler.UseCases.Analysis;

namespace KSPCompiler.Interactors.Analysis;

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

        var noError = messageManger.Count( CompilerMessageLevel.Error ) == 0 &&
                       messageManger.Count( CompilerMessageLevel.Fatal ) == 0;

        return Task.FromResult( CreateOutputData( noError, null ) );

        SemanticAnalysisOutputData CreateOutputData( bool result, Exception? error )
            => new( result, error, new SemanticAnalysisOutputDataDetail( messageManger, node, symbolTable ) );
    }

    public SemanticAnalysisOutputData Execute( SemanticAnalysisInputData input )
        => ExecuteAsync( input ).GetAwaiter().GetResult();
}
