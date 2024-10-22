using System;

using KSPCompiler.Domain.Ast.Analyzers.Semantics;
using KSPCompiler.Domain.CompilerMessages;

using NUnit.Framework;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

[TestFixture]
public class AstVariableDeclarationEvaluationTest
{
    [Test]
    public void DeclarationTest()
    {
        const string name = "$variable";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var symbols = MockUtility.CreateAggregateSymbolTable();

        // Variable can declare in init callback only
        var callbackAst = MockUtility.CreateCallbackDeclarationNode( "init" );
        var ast = MockUtility.CreateVariableDeclarationNode( name );
        ast.Parent = callbackAst;

        var evaluator = new VariableDeclarationEvaluator( compilerMessageManger, symbols.Variables, symbols.UITypes );
        var visitor = new MockDeclarationVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, ast );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.AreEqual( 0, compilerMessageManger.Count( CompilerMessageLevel.Error ) );
        Assert.AreEqual( 1, symbols.Variables.Count );
    }

    [Test]
    public void CannotDeclarationMultipleTest()
    {
        const string name = "$variable";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var symbols = MockUtility.CreateAggregateSymbolTable();

        // Variable can declare in init callback only
        var callbackAst = MockUtility.CreateCallbackDeclarationNode( "init" );
        var ast = MockUtility.CreateVariableDeclarationNode( name );
        ast.Parent = callbackAst;

        var evaluator = new VariableDeclarationEvaluator( compilerMessageManger, symbols.Variables, symbols.UITypes );
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
