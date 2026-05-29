using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.Compilation.Domain;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions;
using KSPCompiler.Shared;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis;

public class SyntaxAnalysisInteractor : ISyntaxAnalysisUseCase
{
    public async Task<Result<SyntaxAnalysisOutput, CompilationFailureReason>> ExecuteAsync( SyntaxAnalysisInput input, CancellationToken cancellationToken = default )
    {
        try
        {
            var ast = input.Parser.Parse();

            await Task.CompletedTask;
            return Result<SyntaxAnalysisOutput, CompilationFailureReason>.Success( new SyntaxAnalysisOutput( ast ) );
        }
        catch( KspScriptParseException e )
        {
            return Result<SyntaxAnalysisOutput, CompilationFailureReason>.Failure( CompilationFailureReason.SyntaxError, e );
        }
        catch( Exception e )
        {
            return Result<SyntaxAnalysisOutput, CompilationFailureReason>.Failure( CompilationFailureReason.Other, e );
        }
    }
}
