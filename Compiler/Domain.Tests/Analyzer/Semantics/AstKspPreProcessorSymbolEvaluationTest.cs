using System;

using KSPCompiler.Domain.Ast.Analyzers.Semantics;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;

using NUnit.Framework;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

[TestFixture]
public class AstKspPreProcessorSymbolEvaluationTest
{
    private AstExpressionNode PreProcessorSymbolTestBody(
        AstSymbolExpressionNode expr,
        AggregateSymbolTable symbolTable,
        ICompilerMessageManger compilerMessageManger )
    {
        var visitor = new MockAstSymbolVisitor();
        var symbolEvaluator = new SymbolEvaluator( compilerMessageManger, symbolTable );
        visitor.Inject( symbolEvaluator );

        var result = visitor.Visit( expr );

        compilerMessageManger.WriteTo( Console.Out );

        return (AstExpressionNode)result;
    }

    [Test]
    public void PreProcessorEvalTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var symbolTable = MockUtility.CreateAggregateSymbolTable();
        var symbol = new KspPreProcessorSymbol{ Name = "TEST" };
        symbolTable.PreProcessorSymbols.Add( symbol );

        var symbolExpr = new AstSymbolExpressionNode( symbol.Name, NullAstExpressionNode.Instance );
        var result = PreProcessorSymbolTestBody(
            symbolExpr,
            symbolTable,
            compilerMessageManger );

        Assert.IsTrue( result.TypeFlag == DataTypeFlag.TypeKspPreprocessorSymbol );
        Assert.IsTrue( compilerMessageManger.Count( CompilerMessageLevel.Error ) == 0 );
    }
}
