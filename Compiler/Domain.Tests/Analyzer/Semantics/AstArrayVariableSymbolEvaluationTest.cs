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
public class AstArrayVariableSymbolEvaluationTest
{
    private AstExpressionNode ArrayVariableSymbolTestBody(
        AstSymbolExpressionNode expr,
        AggregateSymbolTable symbolTable,
        ICompilerMessageManger compilerMessageManger )
    {
        var visitor = new MockAstSymbolVisitor();
        var symbolEvaluator = new SymbolEvaluator( compilerMessageManger, symbolTable );
        visitor.Inject( symbolEvaluator );

        var arrayElementEvaluator = new ArrayElementEvaluator( compilerMessageManger, symbolTable.Variables );
        visitor.Inject( arrayElementEvaluator );

        var result = visitor.Visit( expr );

        compilerMessageManger.WriteTo( Console.Out );

        return (AstExpressionNode)result;
    }

    [TestCase( "%intArray", DataTypeFlag.TypeIntArray )]
    [TestCase( "?realArray", DataTypeFlag.TypeRealArray )]
    [TestCase( "!stringArray", DataTypeFlag.TypeStringArray )]
    public void IntArrayEvalTest( string variableName, DataTypeFlag type )
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var symbolTable = MockUtility.CreateAggregateSymbolTable();
        var variable = MockUtility.CreateVariable( variableName, type );
        variable.ArraySize = 10;
        symbolTable.Variables.Add( variable );

        var symbolExpr = new AstSymbolExpressionNode( variable.Name, NullAstExpressionNode.Instance );
        var result = ArrayVariableSymbolTestBody(
            symbolExpr,
            symbolTable,
            compilerMessageManger );

        Assert.IsTrue( result.TypeFlag == type );
        Assert.IsTrue( compilerMessageManger.Count( CompilerMessageLevel.Error ) == 0 );
    }

    [Test]
    public void IntArrayElementTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var symbolTable = MockUtility.CreateAggregateSymbolTable();
        var variable = MockUtility.CreateVariable( "%x", DataTypeFlag.TypeIntArray );
        variable.ArraySize = 10;
        variable.State     = VariableState.Initialized;
        symbolTable.Variables.Add( variable );

        var symbolExpr = new AstSymbolExpressionNode( variable.Name, NullAstExpressionNode.Instance );
        symbolExpr.Left = new AstArrayElementExpressionNode(
            symbolExpr,
            new AstIntLiteralNode( 0 ),
            NullAstExpressionNode.Instance
        );

        var result = ArrayVariableSymbolTestBody(
            symbolExpr,
            symbolTable,
            compilerMessageManger );

        Assert.IsTrue( result.TypeFlag.IsInt() );
        Assert.IsTrue( compilerMessageManger.Count( CompilerMessageLevel.Error ) == 0 );
    }

    [Test]
    public void RealArrayElementTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var symbolTable = MockUtility.CreateAggregateSymbolTable();
        var variable = MockUtility.CreateVariable( "?x", DataTypeFlag.TypeRealArray );
        variable.ArraySize = 10;
        variable.State     = VariableState.Initialized;
        symbolTable.Variables.Add( variable );

        var symbolExpr = new AstSymbolExpressionNode( variable.Name, NullAstExpressionNode.Instance );
        symbolExpr.Left = new AstArrayElementExpressionNode(
            symbolExpr,
            new AstIntLiteralNode( 0 ),
            NullAstExpressionNode.Instance
        );

        var result = ArrayVariableSymbolTestBody(
            symbolExpr,
            symbolTable,
            compilerMessageManger );

        Assert.IsTrue( result.TypeFlag.IsReal() );
        Assert.IsTrue( compilerMessageManger.Count( CompilerMessageLevel.Error ) == 0 );
    }

    [Test]
    public void StringArrayElementTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var symbolTable = MockUtility.CreateAggregateSymbolTable();
        var variable = MockUtility.CreateVariable( "!x", DataTypeFlag.TypeStringArray );
        variable.ArraySize = 10;
        variable.State     = VariableState.Initialized;
        symbolTable.Variables.Add( variable );

        var symbolExpr = new AstSymbolExpressionNode( variable.Name, NullAstExpressionNode.Instance );
        symbolExpr.Left = new AstArrayElementExpressionNode(
            symbolExpr,
            new AstIntLiteralNode( 0 ),
            NullAstExpressionNode.Instance
        );

        var result = ArrayVariableSymbolTestBody(
            symbolExpr,
            symbolTable,
            compilerMessageManger );

        Assert.IsTrue( result.TypeFlag.IsString() );
        Assert.IsTrue( compilerMessageManger.Count( CompilerMessageLevel.Error ) == 0 );
    }

    [TestCase( 0, 1)]
    [TestCase( 5, -1)]
    [TestCase( 5, 6)]
    public void ArrayOutOfBoundsTest(int arraySize, int arrayIndex )
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var symbolTable = MockUtility.CreateAggregateSymbolTable();
        var variable = MockUtility.CreateVariable( "%x", DataTypeFlag.TypeIntArray );
        variable.ArraySize = arraySize;
        symbolTable.Variables.Add( variable );

        var symbolExpr = new AstSymbolExpressionNode( variable.Name, NullAstExpressionNode.Instance );
        symbolExpr.Left = new AstArrayElementExpressionNode(
            symbolExpr,
            //new AstIntLiteralNode( arrayIndex ),
            new AstRealLiteralNode(0.0),
            NullAstExpressionNode.Instance
        );

        var result = ArrayVariableSymbolTestBody(
            symbolExpr,
            symbolTable,
            compilerMessageManger );

        // Evaluated as int
        Assert.IsTrue( result.TypeFlag.IsInt() );

        // But index out of bounds error detected or array size is zero
        Assert.IsTrue( compilerMessageManger.Count( CompilerMessageLevel.Error ) > 0 );
    }
}
