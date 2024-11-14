using System;

using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.UseCases.Analysis.Context;

namespace KSPCompiler.Interactors.Tests.Analysis.Obfuscators;

public class MockAnalyzerContext : IAnalyzerContext
{
    public ICompilerMessageManger CompilerMessageManger
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
