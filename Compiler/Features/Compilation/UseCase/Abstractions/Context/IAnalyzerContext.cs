using KSPCompiler.Features.Compilation.Domain.Symbols;
using KSPCompiler.Shared.EventEmitting;

namespace KSPCompiler.Features.Compilation.UseCase.Abstractions.Context;

public interface IAnalyzerContext
{
    IEventEmitter EventEmitter { get; }
    AggregateSymbolTable SymbolTable { get; }

    IDeclarationEvaluationContext DeclarationContext { get; }
    IExpressionEvaluatorContext ExpressionContext { get; }
    IStatementEvaluationContext StatementContext { get; }
}
