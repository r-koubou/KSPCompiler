namespace KSPCompiler.UseCases
{
    public interface ISyntaxAnalysis
    {
        SyntaxAnalysisResponse Execute( SyntaxAnalysisRequest request );
    }

    public class SyntaxAnalysisRequest
    {
    }

    public class SyntaxAnalysisResponse
    {
    }
}
