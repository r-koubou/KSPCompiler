using System;

using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Interactors.Analysis.Semantics;

using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

[TestFixture]
public class AstCallbackDeclarationEvaluationTest
{
    [TestCase( "init", true )]
    [TestCase( "init", false )]
    public void DeclarationTest( string name, bool builtIn )
    {
        var compilerMessageManger = ICompilerMessageManger.Default;

        var callback = MockUtility.CreateCallback( name, false );
        callback.BuiltIn = builtIn;

        var symbols = MockUtility.CreateAggregateSymbolTable();

        if( builtIn )
        {
            symbols.BuiltInCallbacks.Add( callback );
        }

        var ast = MockUtility.CreateCallbackDeclarationNode( name );
        var visitor = new MockDeclarationVisitor();
        var evaluator = new CallbackDeclarationEvaluator( compilerMessageManger, symbols.BuiltInCallbacks, symbols.UserCallbacks );

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

        var callback = MockUtility.CreateCallback( name, allowMultiple );
        var symbols = MockUtility.CreateAggregateSymbolTable();
        symbols.BuiltInCallbacks.Add( callback );

        var ast = MockUtility.CreateCallbackDeclarationNode( name );
        var evaluator = new CallbackDeclarationEvaluator( compilerMessageManger, symbols.BuiltInCallbacks, symbols.UserCallbacks );
        var visitor = new MockDeclarationVisitor();

        visitor.Inject( evaluator );

        for( var i = 0; i < 2; i++ )
        {
            evaluator.Evaluate( visitor, ast );
        }

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( allowMultiple ? 0 : 1 ) );
    }

}
