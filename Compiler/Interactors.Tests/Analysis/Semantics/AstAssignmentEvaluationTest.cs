using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.CompilerMessages.Extensions;
using KSPCompiler.Domain.Events;
using KSPCompiler.Domain.Events.Extensions;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Interactors.Analysis.Semantics;
using KSPCompiler.Interactors.Tests.Commons;

using NUnit.Framework;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

[TestFixture]
public class AstAssignmentEvaluationTest
{
    [Test]
    public void IntAssignmentTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var symbolTable = MockUtility.CreateAggregateSymbolTable();
        var visitor = new MockAssignOperatorVisitor();
        var assignEvaluator = new AssignOperatorEvaluator( eventEmitter, symbolTable );
        var variable = MockUtility.CreateSymbolNode( "$x", DataTypeFlag.TypeInt );
        var value = new AstIntLiteralNode( 1 );
        var expr = new AstAssignmentExpressionNode( variable, value );

        visitor.Inject( assignEvaluator );
        var result = visitor.Visit( expr ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 0 ) );
        Assert.That( result, Is.Not.Null );
        Assert.That( result?.TypeFlag, Is.EqualTo( DataTypeFlag.TypeInt ) );
    }

    [Test]
    public void CannotAssignToConstantTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var symbolTable = MockUtility.CreateAggregateSymbolTable();
        var visitor = new MockAssignOperatorVisitor();
        var assignEvaluator = new AssignOperatorEvaluator( eventEmitter, symbolTable );
        var variable = MockUtility.CreateSymbolNode( "$x", DataTypeFlag.TypeInt );
        variable.Constant = true;

        var value = new AstIntLiteralNode( 1 );
        var expr = new AstAssignmentExpressionNode( variable, value );

        visitor.Inject( assignEvaluator );
        var result = visitor.Visit( expr ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.GreaterThan( 0 ) );
        Assert.That( result, Is.Not.Null );
        Assert.That( result?.TypeFlag, Is.EqualTo( DataTypeFlag.TypeInt ) );
    }

    [Test]
    public void CannotAssignToBuiltInVariableTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var symbolTable = MockUtility.CreateAggregateSymbolTable();
        var visitor = new MockAssignOperatorVisitor();
        var assignEvaluator = new AssignOperatorEvaluator( eventEmitter, symbolTable );
        var variable = MockUtility.CreateSymbolNode( "$x", DataTypeFlag.TypeInt );
        variable.BuiltIn = true;

        var value = new AstIntLiteralNode( 1 );
        var expr = new AstAssignmentExpressionNode( variable, value );

        visitor.Inject( assignEvaluator );
        var result = visitor.Visit( expr ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.GreaterThan( 0 ) );
        Assert.That( result, Is.Not.Null );
        Assert.That( result?.TypeFlag, Is.EqualTo( DataTypeFlag.TypeInt ) );
    }

    [Test]
    public void IncompatibleAssignmentTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var symbolTable = MockUtility.CreateAggregateSymbolTable();
        var visitor = new MockAssignOperatorVisitor();
        var assignEvaluator = new AssignOperatorEvaluator( eventEmitter, symbolTable );
        var variable = MockUtility.CreateSymbolNode( "$x", DataTypeFlag.TypeInt );
        var value = MockUtility.CreateSymbolNode( "",      DataTypeFlag.TypeReal | DataTypeFlag.TypeString ); //new AstRealLiteralNode( 1.0 );
        var expr = new AstAssignmentExpressionNode( variable, value );

        visitor.Inject( assignEvaluator );
        var result = visitor.Visit( expr ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.GreaterThan( 0 ) );
        Assert.That( result, Is.Not.Null );
        Assert.That( result?.TypeFlag, Is.EqualTo( DataTypeFlag.TypeInt ) );
    }

    [Test]
    public void ArrayToNoArrayIncompatibleAssignmentTest()
    {
        /*
         * %x := 1 // error: cannot assign value to an array
         */

        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var symbolTable = MockUtility.CreateAggregateSymbolTable();
        var visitor = new MockAssignOperatorVisitor();
        var assignEvaluator = new AssignOperatorEvaluator( eventEmitter, symbolTable );
        var variable = MockUtility.CreateSymbolNode( "%x", DataTypeFlag.TypeIntArray );
        var value = new AstIntLiteralNode( 1 );
        var expr = new AstAssignmentExpressionNode( variable, value );

        visitor.Inject( assignEvaluator );
        var result = visitor.Visit( expr ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.GreaterThan( 0 ) );
        Assert.That( result, Is.Not.Null );
        Assert.That( result?.TypeFlag, Is.EqualTo( DataTypeFlag.TypeIntArray ) );
    }

    [Test]
    public void NoArrayToArrayIncompatibleAssignmentTest()
    {
        /*
         * $x := %y // error: cannot assign an array to a non-array
         */

        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var symbolTable = MockUtility.CreateAggregateSymbolTable();
        var visitor = new MockAssignOperatorVisitor();
        var assignEvaluator = new AssignOperatorEvaluator( eventEmitter, symbolTable );
        var variable = MockUtility.CreateSymbolNode( "$x", DataTypeFlag.TypeInt );
        var value = MockUtility.CreateSymbolNode( "%y",      DataTypeFlag.TypeIntArray );
        var expr = new AstAssignmentExpressionNode( variable, value );

        visitor.Inject( assignEvaluator );
        var result = visitor.Visit( expr ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.GreaterThan( 0 ) );
        Assert.That( result, Is.Not.Null );
        Assert.That( result?.TypeFlag, Is.EqualTo( DataTypeFlag.TypeInt ) );
    }

    [Test]
    public void StringCanAssignmentWithImplicitTest()
    {
        /*
         * @x := 1 // ok: implicit conversion
         */

        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var symbolTable = MockUtility.CreateAggregateSymbolTable();
        var visitor = new MockAssignOperatorVisitor();
        var assignEvaluator = new AssignOperatorEvaluator( eventEmitter, symbolTable );
        var variable = MockUtility.CreateSymbolNode( "@x", DataTypeFlag.TypeString );
        var value = new AstIntLiteralNode( 1 );
        var expr = new AstAssignmentExpressionNode( variable, value );

        visitor.Inject( assignEvaluator );
        var result = visitor.Visit( expr ) as AstExpressionNode;

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 0 ) );
        Assert.That( result, Is.Not.Null );
        Assert.That( result?.TypeFlag, Is.EqualTo( DataTypeFlag.TypeString ) );
    }
}
