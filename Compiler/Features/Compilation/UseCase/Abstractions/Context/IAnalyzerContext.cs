using KSPCompiler.Domain.Symbols;
using KSPCompiler.Gateways.EventEmitting;

namespace KSPCompiler.UseCases.Analysis.Context;

public interface IAnalyzerContext
{
    IEventEmitter EventEmitter { get; }
    AggregateSymbolTable SymbolTable { get; }

    IDeclarationEvaluationContext DeclarationContext { get; }
    IExpressionEvaluatorContext ExpressionContext { get; }
    IStatementEvaluationContext StatementContext { get; }
}
