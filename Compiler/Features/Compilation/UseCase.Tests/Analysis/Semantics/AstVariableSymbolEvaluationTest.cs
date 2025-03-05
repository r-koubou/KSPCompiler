using System;

using KSPCompiler.Features.Compilation.Domain.Messages;
using KSPCompiler.Features.Compilation.Domain.Messages.Extensions;
using KSPCompiler.Features.Compilation.Gateways.EventEmitting;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics;
using KSPCompiler.Features.Compilation.UseCase.Tests.Commons;
using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Shared.Domain.Symbols;
using KSPCompiler.Shared.Domain.Symbols.MetaData;
using KSPCompiler.Shared.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Shared.EventEmitting.Extensions;

using NUnit.Framework;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Semantics;

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

        symbolTable.UserVariables.Add( variable );

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
            variable.Modifier      |= ModifierFlag.Const;
            variable.ConstantValue =  1;
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
        variable.Modifier      |= ModifierFlag.Const;
        variable.ConstantValue =  value;
        variable.State         =  SymbolState.Initialized;

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
        variable.Modifier      |= ModifierFlag.Const;
        variable.ConstantValue =  value;
        variable.State         =  SymbolState.Initialized;

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
        variable.Modifier      |= ModifierFlag.Const;
        variable.ConstantValue =  value;
        variable.State         =  SymbolState.Initialized;

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
