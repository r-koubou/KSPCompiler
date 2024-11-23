using System;

using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Interactors.Analysis.Semantics;

using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

[TestFixture]
public class AstUserFunctionDeclarationEvaluationTest
{
    [Test]
    public void DeclarationTest()
    {
        const string name = "function";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var symbols = MockUtility.CreateAggregateSymbolTable();

        var ast = MockUtility.CreateUserFunctionDeclarationNode( name );
        var evaluator = new UserFunctionDeclarationEvaluator( compilerMessageManger, symbols.UserFunctions );
        var visitor = new MockDeclarationVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, ast );

        compilerMessageManger.WriteTo( Console.Out );

        ClassicAssert.AreEqual( 0, compilerMessageManger.Count( CompilerMessageLevel.Error ) );
        ClassicAssert.AreEqual( 1, symbols.UserFunctions.Count );
    }

    [Test]
    public void CannotDeclarationMultipleTest()
    {
        const string name = "function";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var symbols = MockUtility.CreateAggregateSymbolTable();

        var ast = MockUtility.CreateUserFunctionDeclarationNode( name );
        var evaluator = new UserFunctionDeclarationEvaluator( compilerMessageManger, symbols.UserFunctions );
        var visitor = new MockDeclarationVisitor();

        visitor.Inject( evaluator );

        for( var i = 0; i < 2; i++ )
        {
            evaluator.Evaluate( visitor, ast );
        }

        compilerMessageManger.WriteTo( Console.Out );

        ClassicAssert.AreEqual( 1, compilerMessageManger.Count( CompilerMessageLevel.Error ) );
    }
}
