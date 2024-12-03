using KSPCompiler.Domain.Events;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.UseCases.Analysis.Context;

public interface IAnalyzerContext
{
    IEventEmitter EventEmitter { get; }
    AggregateSymbolTable SymbolTable { get; }

    IDeclarationEvaluationContext DeclarationContext { get; }
    IExpressionEvaluatorContext ExpressionContext { get; }
    IStatementEvaluationContext StatementContext { get; }
}
