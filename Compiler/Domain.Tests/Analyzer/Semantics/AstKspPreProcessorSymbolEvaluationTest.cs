using System;

using KSPCompiler.Domain.Ast.Analyzers.Semantics;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;

using NUnit.Framework;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

[TestFixture]
public class AstKspPreProcessorSymbolEvaluationTest
{
    private AstExpressionNode PreProcessorSymbolTestBody(
        AstSymbolExpressionNode expr,
        AggregateSymbolTable symbolTable,
        ICompilerMessageManger compilerMessageManger,
        AbortTraverseToken abortTraverseToken )
    {
        var visitor = new MockAstSymbolVisitor();
        var symbolEvaluator = new SymbolEvaluator( compilerMessageManger, symbolTable );
        visitor.Inject( symbolEvaluator );

        var result = visitor.Visit( expr, abortTraverseToken );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.IsFalse( abortTraverseToken.Aborted );

        return (AstExpressionNode)result;
    }

    [Test]
    public void PreProcessorEvalTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var abortTraverseToken = new AbortTraverseToken();
        var symbolTable = MockUtility.CreateAggregateSymbolTable();
        var symbol = new KspPreProcessorSymbol{ Name = "TEST" };
        symbolTable.PreProcessorSymbols.Add( symbol );

        var symbolExpr = new AstSymbolExpressionNode( symbol.Name, NullAstExpressionNode.Instance );
        var result = PreProcessorSymbolTestBody(
            symbolExpr,
            symbolTable,
            compilerMessageManger,
            abortTraverseToken
        );

        Assert.IsTrue( result.TypeFlag == DataTypeFlag.TypeKspPreprocessorSymbol );
        Assert.IsTrue( compilerMessageManger.Count( CompilerMessageLevel.Error ) == 0 );
    }
}
