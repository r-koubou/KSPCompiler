namespace KSPCompiler.UseCases
{
    public interface ISemanticAnalysis
    {
        SemanticAnalysisResponse Execute( SemanticAnalysisRequest request );
    }

    public class SemanticAnalysisRequest
    {
    }

    public class SemanticAnalysisResponse
    {
    }
}
