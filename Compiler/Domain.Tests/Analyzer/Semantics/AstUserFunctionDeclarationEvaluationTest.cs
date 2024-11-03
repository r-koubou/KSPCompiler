using System;

using KSPCompiler.Domain.Ast.Analyzers.Semantics;
using KSPCompiler.Domain.CompilerMessages;

using NUnit.Framework;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

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

        Assert.AreEqual( 0, compilerMessageManger.Count( CompilerMessageLevel.Error ) );
        Assert.AreEqual( 1, symbols.UserFunctions.Count );
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

        Assert.AreEqual( 1, compilerMessageManger.Count( CompilerMessageLevel.Error ) );
    }
}
