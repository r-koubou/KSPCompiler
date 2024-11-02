using System;

using KSPCompiler.Domain.Ast.Analyzers.Semantics;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols.MetaData;

using NUnit.Framework;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

[TestFixture]
public class AstAssignmentEvaluationTest
{
    [Test]
    public void IntAssignmentTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAssignOperatorVisitor();
        var assignEvaluator = new AssignOperatorEvaluator( compilerMessageManger );
        var variable = MockUtility.CreateSymbolNode( "$x", DataTypeFlag.TypeInt );
        var value = new AstIntLiteralNode( 1 );
        var expr = new AstAssignmentExpressionNode( variable, value );

        visitor.Inject( assignEvaluator );
        var result = visitor.Visit( expr ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        Assert.IsFalse( compilerMessageManger.Count( CompilerMessageLevel.Error ) > 0 );
        Assert.IsNotNull( result );
        Assert.AreEqual( DataTypeFlag.TypeInt, result?.TypeFlag );
    }

    [Test]
    public void CannotAssignToConstantTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAssignOperatorVisitor();
        var assignEvaluator = new AssignOperatorEvaluator( compilerMessageManger );
        var variable = MockUtility.CreateSymbolNode( "$x", DataTypeFlag.TypeInt );
        variable.Constant = true;

        var value = new AstIntLiteralNode( 1 );
        var expr = new AstAssignmentExpressionNode( variable, value );

        visitor.Inject( assignEvaluator );
        var result = visitor.Visit( expr ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        Assert.IsTrue( compilerMessageManger.Count( CompilerMessageLevel.Error ) > 0 );
        Assert.IsNotNull( result );
        Assert.AreEqual( DataTypeFlag.TypeInt, result?.TypeFlag );
    }

    [Test]
    public void IncompatibleAssignmentTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAssignOperatorVisitor();
        var assignEvaluator = new AssignOperatorEvaluator( compilerMessageManger );
        var variable = MockUtility.CreateSymbolNode( "$x", DataTypeFlag.TypeInt );
        var value = MockUtility.CreateSymbolNode( "",      DataTypeFlag.TypeReal | DataTypeFlag.TypeString ); //new AstRealLiteralNode( 1.0 );
        var expr = new AstAssignmentExpressionNode( variable, value );

        visitor.Inject( assignEvaluator );
        var result = visitor.Visit( expr ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        Assert.IsTrue( compilerMessageManger.Count( CompilerMessageLevel.Error ) > 0 );
        Assert.IsNotNull( result );
        Assert.AreEqual( DataTypeFlag.TypeInt, result?.TypeFlag );
    }

    [Test]
    public void ArrayToNoArrayIncompatibleAssignmentTest()
    {
        /*
         * %x := 1 // error: cannot assign value to an array
         */

        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAssignOperatorVisitor();
        var assignEvaluator = new AssignOperatorEvaluator( compilerMessageManger );
        var variable = MockUtility.CreateSymbolNode( "%x", DataTypeFlag.TypeIntArray );
        var value = new AstIntLiteralNode( 1 );
        var expr = new AstAssignmentExpressionNode( variable, value );

        visitor.Inject( assignEvaluator );
        var result = visitor.Visit( expr ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        Assert.IsTrue( compilerMessageManger.Count( CompilerMessageLevel.Error ) > 0 );
        Assert.IsNotNull( result );
        Assert.AreEqual( DataTypeFlag.TypeIntArray, result?.TypeFlag );
    }

    [Test]
    public void NoArrayToArrayIncompatibleAssignmentTest()
    {
        /*
         * $x := %y // error: cannot assign an array to a non-array
         */

        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAssignOperatorVisitor();
        var assignEvaluator = new AssignOperatorEvaluator( compilerMessageManger );
        var variable = MockUtility.CreateSymbolNode( "$x", DataTypeFlag.TypeInt );
        var value = MockUtility.CreateSymbolNode( "%y",      DataTypeFlag.TypeIntArray );
        var expr = new AstAssignmentExpressionNode( variable, value );

        visitor.Inject( assignEvaluator );
        var result = visitor.Visit( expr ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        Assert.IsTrue( compilerMessageManger.Count( CompilerMessageLevel.Error ) > 0 );
        Assert.IsNotNull( result );
        Assert.AreEqual( DataTypeFlag.TypeInt, result?.TypeFlag );
    }

    [Test]
    public void StringCanAssignmentWithImplicitTest()
    {
        /*
         * @x := 1 // ok: implicit conversion
         */

        var compilerMessageManger = ICompilerMessageManger.Default;
        var visitor = new MockAssignOperatorVisitor();
        var assignEvaluator = new AssignOperatorEvaluator( compilerMessageManger );
        var variable = MockUtility.CreateSymbolNode( "@x", DataTypeFlag.TypeString );
        var value = new AstIntLiteralNode( 1 );
        var expr = new AstAssignmentExpressionNode( variable, value );

        visitor.Inject( assignEvaluator );
        var result = visitor.Visit( expr ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        Assert.IsFalse( compilerMessageManger.Count( CompilerMessageLevel.Error ) > 0 );
        Assert.IsNotNull( result );
        Assert.AreEqual( DataTypeFlag.TypeString, result?.TypeFlag );
    }
}
