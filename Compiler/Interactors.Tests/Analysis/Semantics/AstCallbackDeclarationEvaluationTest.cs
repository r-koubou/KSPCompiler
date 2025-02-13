using System;

using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.CompilerMessages.Extensions;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Gateways.EventEmitting;
using KSPCompiler.Gateways.EventEmitting.Extensions;
using KSPCompiler.Interactors.Analysis.Semantics;
using KSPCompiler.Interactors.Tests.Commons;

using NUnit.Framework;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

[TestFixture]
public class AstCallbackDeclarationEvaluationTest
{
    [TestCase( "init", true )]
    [TestCase( "init", false )]
    public void DeclarationTest( string name, bool builtIn )
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var callback = MockUtility.CreateCallback( name, false );
        callback.BuiltIn = builtIn;

        var symbols = MockUtility.CreateAggregateSymbolTable();

        if( builtIn )
        {
            symbols.BuiltInCallbacks.AddAsNoOverload( callback );
        }

        var ast = MockUtility.CreateCallbackDeclarationNode( name );
        var visitor = new MockDeclarationVisitor();
        var evaluator = new CallbackDeclarationEvaluator( eventEmitter, symbols );

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, ast );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 0 ) );
        Assert.That( symbols.UserCallbacks.Count, Is.EqualTo( 1 ) );
    }

    [TestCase( "init", false )]
    [TestCase( "init", true )]
    public void CannotDeclarationNonAllowMultipleTest( string name, bool allowMultiple )
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var overload = new CallbackArgumentSymbolList
        {
            new CallbackArgumentSymbol( true ) { Name = new SymbolName( "arg1" ) }
        };

        var callback = MockUtility.CreateCallback( name, allowMultiple );
        var symbols = MockUtility.CreateAggregateSymbolTable();

        if( allowMultiple )
        {
            symbols.BuiltInCallbacks.AddAsOverload( callback, overload );
        }
        else
        {
            symbols.BuiltInCallbacks.AddAsNoOverload( callback );
        }

        var ast = MockUtility.CreateCallbackDeclarationNode( name );
        var evaluator = new CallbackDeclarationEvaluator( eventEmitter, symbols );
        var visitor = new MockDeclarationVisitor();

        visitor.Inject( evaluator );

        for( var i = 0; i < 2; i++ )
        {
            evaluator.Evaluate( visitor, ast );
        }

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 1 ) );

        return;

        CallbackArgumentSymbolList CreateOverload( string argName )
        {
            var result = new CallbackArgumentSymbolList
            {
                new CallbackArgumentSymbol( true ) { Name = new SymbolName( argName ) }
            };

            return result;
        }
    }

}
