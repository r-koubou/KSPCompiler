namespace KSPCompiler.Domain.Ast.Analyzers.Context;

public interface IAnalyzerContext
{
    IDeclarationEvaluationContext DeclarationContext { get; }
    IExpressionEvaluatorContext ExpressionContext { get; }
    IStatementEvaluationContext StatementContext { get; }
}
