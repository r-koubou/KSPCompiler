using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.Compilation.Domain;
using KSPCompiler.Features.Compilation.UseCase.Abstractions;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis;

public class SyntaxAnalysisInteractor : ISyntaxAnalysisUseCase
{
    public SyntaxAnalysisOutputData Execute( SyntaxAnalysisInputData input )
        => ExecuteAsync( input ).GetAwaiter().GetResult();

    public Task<SyntaxAnalysisOutputData> ExecuteAsync( SyntaxAnalysisInputData parameter, CancellationToken cancellationToken = default )
    {
        try
        {
            var ast = parameter.Input.Parse();
            return Task.FromResult( new SyntaxAnalysisOutputData( ast, true, null ) );
        }
        catch( KspScriptParseException e )
        {
            return Task.FromResult( new SyntaxAnalysisOutputData( default!, false, e ) );
        }
    }
}
