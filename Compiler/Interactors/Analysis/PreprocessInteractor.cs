using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Interactors.Analysis.Preprocessing;
using KSPCompiler.UseCases.Analysis;

namespace KSPCompiler.Interactors.Analysis;

public class PreprocessInteractor : IPreprocessUseCase
{
    public Task<PreprocessOutputData> ExecuteAsync( PreprocessInputData parameter, CancellationToken cancellationToken = default )
    {
        var messageManger = parameter.InputData.MessageManager;
        var node = parameter.InputData.CompilationUnitNode;
        var symbolTable = parameter.InputData.SymbolTable;

        var preprocessor = new PreprocessAnalyzer( symbolTable.PreProcessorSymbols );

        try
        {
            preprocessor.Traverse( parameter.InputData.CompilationUnitNode );
        }
        catch( Exception e )
        {
            return Task.FromResult( CreateOutputData( false, e ) );
        }

        return Task.FromResult( CreateOutputData( false, null ) );

        PreprocessOutputData CreateOutputData( bool result, Exception? error )
            => new( result, error, new PreprocessOutputDataDetail( messageManger, node, symbolTable ) );
    }
}
