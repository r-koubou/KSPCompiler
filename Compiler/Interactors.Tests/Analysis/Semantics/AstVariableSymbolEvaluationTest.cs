using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.CompilerMessages.Extensions;
using KSPCompiler.Domain.Events;
using KSPCompiler.Domain.Events.Extensions;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Interactors.Analysis.Semantics;
using KSPCompiler.Interactors.Tests.Commons;

using NUnit.Framework;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

[TestFixture]
public class AstVariableSymbolEvaluationTest
{
    private static AstExpressionNode VariableSymbolTestBody( VariableSymbol variable )
    {
        var visitor = new MockAstSymbolVisitor();
        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var symbolTable = MockUtility.CreateAggregateSymbolTable();

        symbolTable.BuiltInVariables.Add( variable );

        var symbolEvaluator = new SymbolEvaluator( eventEmitter, symbolTable );
        visitor.Inject( symbolEvaluator );

        var node = MockUtility.CreateSymbolNode( variable.Name );
        var result = visitor.Visit( node );

        compilerMessageManger.WriteTo( Console.Out );

        return (AstExpressionNode)result;
    }

    [TestCase( false )]
    [TestCase( true )]
    public void IntVariableSymbolTest( bool isConstant )
    {
        var variable = MockUtility.CreateVariable( "$x", DataTypeFlag.TypeInt );

        if( isConstant )
        {
            variable.Modifier |= ModifierFlag.Const;
            variable.Value            =  1;
        }

        variable.State = SymbolState.Initialized;

        var result = VariableSymbolTestBody( variable );

        Assert.That(
            result,
            isConstant
                ? Is.InstanceOf<AstIntLiteralNode>()
                : Is.InstanceOf<AstSymbolExpressionNode>()
        );


        Assert.That( result.TypeFlag.IsInt(), Is.True );
    }

    [TestCase( 123)]
    [TestCase( -123)]
    public void EvaluateConvolutionIntVariableTest( int value )
    {
        var variable = MockUtility.CreateVariable( "$x", DataTypeFlag.TypeInt );
        variable.Modifier |= ModifierFlag.Const;
        variable.Value    =  value;
        variable.State    = SymbolState.Initialized;

        var result = VariableSymbolTestBody( variable );
        var literal = result as AstIntLiteralNode;

        Assert.That( result.Constant, Is.True );
        Assert.That( literal, Is.Not.Null );
        Assert.That( literal?.Value, Is.EqualTo( value ) );
    }

    [TestCase( 1.23)]
    [TestCase( -1.23)]
    public void EvaluateConvolutionRealVariableTest( double value )
    {
        var variable = MockUtility.CreateRealVariable( "~x" );
        variable.Modifier |= ModifierFlag.Const;
        variable.Value    =  value;
        variable.State    =  SymbolState.Initialized;

        var result = VariableSymbolTestBody( variable );
        var literal = result as AstRealLiteralNode;

        Assert.That( result.Constant, Is.True );
        Assert.That( literal, Is.Not.Null );
        Assert.That( literal?.Value, Is.EqualTo( value ) );
    }

    [TestCase( "abc")]
    [TestCase( "ABC")]
    public void EvaluateConvolutionStringVariableTest( string value )
    {
        var variable = MockUtility.CreateStringVariable( "@x" );
        variable.Modifier |= ModifierFlag.Const;
        variable.Value    =  value;
        variable.State    =  SymbolState.Initialized;

        var result = VariableSymbolTestBody( variable );
        var literal = result as AstStringLiteralNode;

        Assert.That( result.Constant, Is.True );
        Assert.That( literal, Is.Not.Null );
        Assert.That( literal?.Value, Is.EqualTo( value ) );
    }

    [Test]
    public void CannotEvaluateNonDeclaredVariableTest()
    {
        var visitor = new MockAstSymbolVisitor();
        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var symbolTable = MockUtility.CreateAggregateSymbolTable(); // no variables registered

        var symbolEvaluator = new SymbolEvaluator( eventEmitter, symbolTable );
        visitor.Inject( symbolEvaluator );

        var node = MockUtility.CreateSymbolNode( "$x" );
        visitor.Visit( node );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 1 ) );
    }
}
