using System;

using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Interactors.Analysis.Semantics;

using NUnit.Framework;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

[TestFixture]
public class AstCallbackDeclarationEvaluationTest
{
    [TestCase( "init", true )]
    [TestCase( "init", false )]
    public void DeclarationTest( string name, bool reserved )
    {
        var compilerMessageManger = ICompilerMessageManger.Default;

        var callback = MockUtility.CreateCallback( name, false );
        callback.BuiltIn = reserved;

        var symbols = MockUtility.CreateAggregateSymbolTable();

        if( reserved )
        {
            symbols.BuiltInCallbacks.Add( callback );
        }

        var ast = MockUtility.CreateCallbackDeclarationNode( name );
        var visitor = new MockDeclarationVisitor();
        var evaluator = new CallbackDeclarationEvaluator( compilerMessageManger, symbols.BuiltInCallbacks, symbols.UserCallbacks );

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, ast );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.AreEqual( 0, compilerMessageManger.Count( CompilerMessageLevel.Error ) );
        Assert.AreEqual( 1, symbols.UserCallbacks.Count );
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

        Assert.AreEqual( allowMultiple ? 0 : 1, compilerMessageManger.Count( CompilerMessageLevel.Error ) );
    }

}
