using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Domain.Ast.Analyzers.Context;

public interface IAnalyzerContext
{
    ICompilerMessageManger CompilerMessageManger { get; }
    AggregateSymbolTable SymbolTable { get; }

    IDeclarationEvaluationContext DeclarationContext { get; }
    IExpressionEvaluatorContext ExpressionContext { get; }
    IStatementEvaluationContext StatementContext { get; }
}
