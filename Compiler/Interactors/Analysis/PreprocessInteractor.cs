using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Interactors.Analysis.Preprocessing;
using KSPCompiler.UseCases;
using KSPCompiler.UseCases.Analysis;

namespace KSPCompiler.Interactors.Analysis;

public class PreprocessInteractor : IPreprocessUseCase
{
    public async Task<UnitOutputPort> ExecuteAsync( PreprocessInputData parameter, CancellationToken cancellationToken = default )
    {
        var eventDispatcher = parameter.InputData.EventDispatcher;
        var ast = parameter.InputData.CompilationUnitNode;
        var symbolTable = parameter.InputData.SymbolTable;

        var preprocessor = new PreprocessAnalyzer( symbolTable.PreProcessorSymbols, eventDispatcher );

        try
        {
            preprocessor.Traverse( ast );
            await Task.CompletedTask;
        }
        catch( Exception e )
        {
            return new UnitOutputPort( false, e );
        }

        return new UnitOutputPort( true );
    }
}
