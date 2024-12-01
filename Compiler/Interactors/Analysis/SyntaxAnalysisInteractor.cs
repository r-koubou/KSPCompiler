using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Domain;
using KSPCompiler.UseCases.Analysis;

namespace KSPCompiler.Interactors.Analysis;

public class SyntaxAnalysisInteractor : ISyntaxAnalysisUseCase
{
    public Task<SyntaxAnalysisOutputData> ExecuteAsync( SyntaxAnalysisInputData parameter, CancellationToken cancellationToken = default )
    {
        try
        {
            var ast = parameter.InputData.Parse();
            return Task.FromResult( new SyntaxAnalysisOutputData( true, null, ast ) );
        }
        catch( KspScriptParseException e )
        {
            return Task.FromResult( new SyntaxAnalysisOutputData( false, e, default! ) );
        }
    }
}
