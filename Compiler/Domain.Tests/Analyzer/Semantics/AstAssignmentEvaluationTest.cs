using System;

using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Operators;
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
        var value = new AstRealLiteralNode( 1.0 );
        var expr = new AstAssignmentExpressionNode( variable, value );

        visitor.Inject( assignEvaluator );
        var result = visitor.Visit( expr ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        Assert.IsTrue( compilerMessageManger.Count( CompilerMessageLevel.Error ) > 0 );
        Assert.IsNotNull( result );
        Assert.AreEqual( DataTypeFlag.TypeInt, result?.TypeFlag );
    }

    [Test]
    public void DirectlyArrayAccessAssignmentTest()
    {
        /*
         * %x := 1 // error: cannot assign to an array without an index
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
}

public class MockAssignOperatorVisitor : DefaultAstVisitor
{
    private IAssignOperatorEvaluator AssignOperatorEvaluator { get; set; } = new MockAssignOperatorEvaluator();

    public void Inject( IAssignOperatorEvaluator evaluator )
    {
        AssignOperatorEvaluator = evaluator;
    }

    public override IAstNode Visit( AstAssignmentExpressionNode node )
        => AssignOperatorEvaluator.Evaluate( this, node );
}

public class MockAssignOperatorEvaluator : IAssignOperatorEvaluator
{
    public IAstNode Evaluate( IAstVisitor<IAstNode> visitor, AstExpressionNode expr )
    {
        throw new NotImplementedException();
    }
}
