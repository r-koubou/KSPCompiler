using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Domain;
using KSPCompiler.UseCases.Analysis;

namespace KSPCompiler.Interactors.Analysis;

public class SyntaxAnalysisInteractor : ISyntaxAnalysisUseCase
{
    public SyntaxAnalysisOutputData Execute( SyntaxAnalysisInputData input )
        => ExecuteAsync( input ).GetAwaiter().GetResult();

    public Task<SyntaxAnalysisOutputData> ExecuteAsync( SyntaxAnalysisInputData parameter, CancellationToken cancellationToken = default )
    {
        try
        {
            var ast = parameter.HandlingInputData.Parse();
            return Task.FromResult( new SyntaxAnalysisOutputData( ast, true, null ) );
        }
        catch( KspScriptParseException e )
        {
            return Task.FromResult( new SyntaxAnalysisOutputData( default!, false, e ) );
        }
    }
}
