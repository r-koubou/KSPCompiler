using System;

using KSPCompiler.Features.Compilation.Domain.Messages;
using KSPCompiler.Features.Compilation.Domain.Messages.Extensions;
using KSPCompiler.Features.Compilation.Gateways.EventEmitting;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics;
using KSPCompiler.Features.Compilation.UseCase.Tests.Commons;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.EventEmitting.Extensions;

using NUnit.Framework;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Semantics;

[TestFixture]
public class AstCallUserFunctionEvaluationTest
{
    [Test]
    public void CallFunctionTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var symbols = new AggregateSymbolTable();

        // register function `my_function`
        var function = MockUtility.CreateUserFunction( "my_function" );

        // register the function
        symbols.UserFunctions.Add( function );

        // Create a call function statement node
        // call my_function
        var callFunctionAst = MockUtility.CreateCallUserFunctionNode( function );

        var evaluator = new CallUserFunctionEvaluator( eventEmitter, symbols );
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

        var symbols = new AggregateSymbolTable();

        // Don't register the function for make a error
        // var function = MockUtility.CreateUserFunction( "my_function" );
        // symbols.UserFunctions.Add( function );

        // Creation of a non-existent call command expression node
        var callFunctionAst = MockUtility.CreateCallUserFunctionNode( "my_function" );

        var evaluator = new CallUserFunctionEvaluator( eventEmitter, symbols );
        var visitor = new MockCallUserFunctionStatementVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, callFunctionAst );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 1 ) );
    }
}
