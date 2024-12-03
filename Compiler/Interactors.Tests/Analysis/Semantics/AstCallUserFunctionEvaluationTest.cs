using System;

using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.CompilerMessages.Extensions;
using KSPCompiler.Domain.Events;
using KSPCompiler.Domain.Events.Extensions;
using KSPCompiler.Interactors.Analysis.Semantics;
using KSPCompiler.Interactors.Tests.Commons;

using NUnit.Framework;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

[TestFixture]
public class AstCallUserFunctionEvaluationTest
{
    [Test]
    public void CallFunctionTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var symbols = MockUtility.CreateAggregateSymbolTable();

        // register function `my_function`
        var function = MockUtility.CreateUserFunction( "my_function" );

        // register the function
        symbols.UserFunctions.Add( function );

        // Create a call function statement node
        // call my_function
        var callFunctionAst = MockUtility.CreateCallUserFunctionNode( function );

        var evaluator = new CallUserFunctionEvaluator( eventEmitter, symbols.UserFunctions );
        var visitor = new MockCallUserFunctionStatementVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, callFunctionAst );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 0 ) );
    }

    [Test]
    public void CannotCallNotRegisteredFunctionTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var symbols = MockUtility.CreateAggregateSymbolTable();

        // Don't register the function for make a error
        // var function = MockUtility.CreateUserFunction( "my_function" );
        // symbols.UserFunctions.Add( function );

        // Creation of a non-existent call command expression node
        var callFunctionAst = MockUtility.CreateCallUserFunctionNode( "my_function" );

        var evaluator = new CallUserFunctionEvaluator( eventEmitter, symbols.UserFunctions );
        var visitor = new MockCallUserFunctionStatementVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, callFunctionAst );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 1 ) );
    }
}
