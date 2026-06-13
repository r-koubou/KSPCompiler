using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Commons.Evaluations;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Preprocessing;
using KSPCompiler.Shared;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis;

public class PreprocessInteractor : IPreprocessUseCase
{
    public async Task<Result<Unit, CompilationFailureReason>> ExecuteAsync( PreprocessInput input, CancellationToken cancellationToken = default )
    {
        var eventEmitter = input.EventEmitter;
        var ast = input.CompilationUnitNode;
        var symbolTable = input.SymbolTable;

        var preprocessor = new PreprocessAnalyzer( symbolTable.PreProcessorSymbols, eventEmitter );

        try
        {
            preprocessor.Traverse( ast );
            await Task.CompletedTask;
            return Result<Unit, CompilationFailureReason>.Success( Unit.Default );
        }
        catch( AstAnalyzeException e )
        {
            return Result<Unit, CompilationFailureReason>.Failure( CompilationFailureReason.SemanticsError, e );
        }
        catch( Exception e )
        {
            return Result<Unit, CompilationFailureReason>.Failure( CompilationFailureReason.Other, e );
        }
    }
}
