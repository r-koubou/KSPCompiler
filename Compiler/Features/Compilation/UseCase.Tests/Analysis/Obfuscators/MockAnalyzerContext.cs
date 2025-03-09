using System;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Context;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.EventEmitting;

namespace KSPCompiler.Features.SymbolManagement.UseCase.Tests.Analysis.Obfuscators;

public class MockAnalyzerContext : IAnalyzerContext
{
    public IEventEmitter EventEmitter
        => throw new NotImplementedException();

    public AggregateSymbolTable SymbolTable
        => throw new NotImplementedException();

    public IDeclarationEvaluationContext DeclarationContext
        => throw new NotImplementedException();
    public IExpressionEvaluatorContext ExpressionContext
        => throw new NotImplementedException();

    public IStatementEvaluationContext StatementContext
        => throw new NotImplementedException();
}
