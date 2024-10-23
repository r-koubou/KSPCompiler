using System;

using KSPCompiler.Domain.Ast.Analyzers.Semantics;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
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

    [Test]
    public void PrimitiveDeclarationWithInitializerTest()
    {
        const string name = "$variable";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var symbols = MockUtility.CreateAggregateSymbolTable();

        // Variable can declare in init callback only
        var callbackAst = MockUtility.CreateCallbackDeclarationNode( "init" );

        // declare $variable
        var declaration = MockUtility.CreateVariableDeclarationNode( name );
        declaration.Parent = callbackAst;

        // := 1
        declaration.Initializer = new AstVariableInitializerNode( declaration )
        {
            PrimitiveInitializer = new AstPrimitiveInitializerNode(
                declaration,
                new AstIntLiteralNode( 1 ),
                NullAstExpressionListNode.Instance
            )
        };

        var evaluator = new VariableDeclarationEvaluator( compilerMessageManger, symbols.Variables, symbols.UITypes );
        var visitor = new MockDeclarationVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, declaration );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.AreEqual( 0, compilerMessageManger.Count( CompilerMessageLevel.Error ) );
        Assert.AreEqual( 1, symbols.Variables.Count );
    }

    [Test]
    public void PrimitiveDeclarationWithIncompatibleTypeInitializerTest()
    {
        const string name = "$variable";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var symbols = MockUtility.CreateAggregateSymbolTable();

        // Variable can declare in init callback only
        var callbackAst = MockUtility.CreateCallbackDeclarationNode( "init" );

        // declare $variable
        var declaration = MockUtility.CreateVariableDeclarationNode( name );
        declaration.Parent = callbackAst;

        // := 1
        declaration.Initializer = new AstVariableInitializerNode( declaration )
        {
            PrimitiveInitializer = new AstPrimitiveInitializerNode(
                declaration,
                new AstStringLiteralNode("string"),
                NullAstExpressionListNode.Instance
            )
        };

        var evaluator = new VariableDeclarationEvaluator( compilerMessageManger, symbols.Variables, symbols.UITypes );
        var visitor = new MockDeclarationVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, declaration );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.AreEqual( 1, compilerMessageManger.Count( CompilerMessageLevel.Error ) );
        Assert.AreEqual( 0, symbols.Variables.Count );
    }

    [Test]
    public void ArrayDeclarationWithInitializerTest()
    {
        const string name = "%variable";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var symbols = MockUtility.CreateAggregateSymbolTable();

        // Variable can declare in init callback only
        var callbackAst = MockUtility.CreateCallbackDeclarationNode( "init" );

        // declare %variable
        var declaration = MockUtility.CreateVariableDeclarationNode( name );
        declaration.Parent = callbackAst;

        // [10] := (1, 2, 3, 4, 5, 6, 7, 8, 9, 10)
        declaration.Initializer = new AstVariableInitializerNode( declaration )
        {
            ArrayInitializer = new AstArrayInitializerNode( declaration )
            {
                Size = new AstIntLiteralNode( 10 )
            }
        };

        for( var i = 0; i < 10; i++ )
        {
            declaration.Initializer.ArrayInitializer.Initializer.Expressions.Add(
                new AstIntLiteralNode( i + 1 )
            );
        }

        var evaluator = new VariableDeclarationEvaluator( compilerMessageManger, symbols.Variables, symbols.UITypes );
        var visitor = new MockDeclarationVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, declaration );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.AreEqual( 0, compilerMessageManger.Count( CompilerMessageLevel.Error ) );
        Assert.AreEqual( 1, symbols.Variables.Count );
    }
}
