using KSPCompiler.Shared.Domain.Symbols;
using KSPCompiler.Shared.EventEmitting;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Context;

public interface IAnalyzerContext
{
    IEventEmitter EventEmitter { get; }
    AggregateSymbolTable SymbolTable { get; }

    IDeclarationEvaluationContext DeclarationContext { get; }
    IExpressionEvaluatorContext ExpressionContext { get; }
    IStatementEvaluationContext StatementContext { get; }
}
