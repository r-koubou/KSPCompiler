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
public class AstPgsSymbolEvaluationTest
{
    private AstExpressionNode PgsSymbolTestBody(
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
    public void PgsSymbolEvalTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var symbolTable = MockUtility.CreateAggregateSymbolTable();

        var symbolExpr = new AstSymbolExpressionNode( "TEST", NullAstExpressionNode.Instance );
        var result = PgsSymbolTestBody(
            symbolExpr,
            symbolTable,
            compilerMessageManger );

        Assert.IsTrue( result.TypeFlag == DataTypeFlag.TypePgsId );
        Assert.IsTrue( compilerMessageManger.Count( CompilerMessageLevel.Error ) == 0 );
    }
}
