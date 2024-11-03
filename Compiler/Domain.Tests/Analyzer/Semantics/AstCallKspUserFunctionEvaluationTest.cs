using System;

using KSPCompiler.Domain.Ast.Analyzers.Semantics;
using KSPCompiler.Domain.CompilerMessages;

using NUnit.Framework;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

[TestFixture]
public class AstCallKspUserFunctionEvaluationTest
{
    [Test]
    public void CallFunctionTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var symbols = MockUtility.CreateAggregateSymbolTable();

        // register function `my_function`
        var function = MockUtility.CreateKspUserFunction( "my_function" );

        // register the function
        symbols.UserFunctions.Add( function );

        // Create a call function statement node
        // call my_function
        var callFunctionAst = MockUtility.CreateCallKspUserFunctionNode( function );

        var evaluator = new CallKspUserFunctionStatementEvaluator( compilerMessageManger, symbols.UserFunctions );
        var visitor = new MockCallKspUserFunctionStatementVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, callFunctionAst );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.AreEqual( 0, compilerMessageManger.Count( CompilerMessageLevel.Error ) );
    }

    [Test]
    public void CannotCallNotRegisteredFunctionTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var symbols = MockUtility.CreateAggregateSymbolTable();

        // Don't register the function for make a error
        // var function = MockUtility.CreateKspUserFunction( "my_function" );
        // symbols.UserFunctions.Add( function );

        // Creation of a non-existent call command expression node
        var callFunctionAst = MockUtility.CreateCallKspUserFunctionNode( "my_function" );

        var evaluator = new CallKspUserFunctionStatementEvaluator( compilerMessageManger, symbols.UserFunctions );
        var visitor = new MockCallKspUserFunctionStatementVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, callFunctionAst );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.AreEqual( 1, compilerMessageManger.Count( CompilerMessageLevel.Error ) );
    }
}
