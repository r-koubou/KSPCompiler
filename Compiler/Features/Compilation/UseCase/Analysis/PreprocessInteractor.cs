using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Preprocessing;
using KSPCompiler.Shared.UseCase;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis;

public class PreprocessInteractor : IPreprocessUseCase
{
    public async Task<UnitOutputPort> ExecuteAsync( PreprocessInputData parameter, CancellationToken cancellationToken = default )
    {
        var eventEmitter = parameter.Input.EventEmitter;
        var ast = parameter.Input.CompilationUnitNode;
        var symbolTable = parameter.Input.SymbolTable;

        var preprocessor = new PreprocessAnalyzer( symbolTable.PreProcessorSymbols, eventEmitter );

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
