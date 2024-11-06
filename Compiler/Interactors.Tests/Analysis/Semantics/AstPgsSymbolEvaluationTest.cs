using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Interactors.Analysis.Semantics;

using NUnit.Framework;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

[TestFixture]
public class AstPgsSymbolEvaluationTest
{
    private void PgsSymbolTestBody(
        AstSymbolExpressionNode expr,
        int expectedErrorCount )
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAstSymbolVisitor();
        var symbolTable = MockUtility.CreateAggregateSymbolTable();

        var symbolEvaluator = new SymbolEvaluator( compilerMessageManger, symbolTable );
        visitor.Inject( symbolEvaluator );

        var result = visitor.Visit( expr );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.IsTrue( result is AstExpressionNode );
        Assert.IsTrue( ((AstExpressionNode)result).TypeFlag == DataTypeFlag.TypePgsId );
        Assert.AreEqual( expectedErrorCount, compilerMessageManger.Count( CompilerMessageLevel.Error ) );
    }

    [Test]
    public void PgsSymbolEvalTest()
    {
        var symbolExpr = new AstSymbolExpressionNode( "TEST", NullAstExpressionNode.Instance );
        PgsSymbolTestBody( symbolExpr, 0 );
    }

    [Test]
    public void CannotEvaluatePgsIdIsOver64Characters()
    {
        var over64Chars = new string( 'A', 65 );
        var symbolExpr = new AstSymbolExpressionNode( over64Chars, NullAstExpressionNode.Instance );
        PgsSymbolTestBody( symbolExpr, 1 );
    }
}
