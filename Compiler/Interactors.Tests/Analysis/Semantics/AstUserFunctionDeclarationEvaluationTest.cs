using System;

using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.CompilerMessages.Extensions;
using KSPCompiler.Gateways.EventEmitting;
using KSPCompiler.Gateways.EventEmitting.Extensions;
using KSPCompiler.Interactors.Analysis.Semantics;
using KSPCompiler.Interactors.Tests.Commons;

using NUnit.Framework;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

[TestFixture]
public class AstUserFunctionDeclarationEvaluationTest
{
    [Test]
    public void DeclarationTest()
    {
        const string name = "function";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var symbols = MockUtility.CreateAggregateSymbolTable();

        var ast = MockUtility.CreateUserFunctionDeclarationNode( name );
        var evaluator = new UserFunctionDeclarationEvaluator( eventEmitter, symbols );
        var visitor = new MockDeclarationVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, ast );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 0 ) );
        Assert.That( symbols.UserFunctions.Count, Is.EqualTo( 1 ) );
    }

    [Test]
    public void CannotDeclarationMultipleTest()
    {
        const string name = "function";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var symbols = MockUtility.CreateAggregateSymbolTable();

        var ast = MockUtility.CreateUserFunctionDeclarationNode( name );
        var evaluator = new UserFunctionDeclarationEvaluator( eventEmitter, symbols );
        var visitor = new MockDeclarationVisitor();

        visitor.Inject( evaluator );

        for( var i = 0; i < 2; i++ )
        {
            evaluator.Evaluate( visitor, ast );
        }

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 1 ) );
    }
}
