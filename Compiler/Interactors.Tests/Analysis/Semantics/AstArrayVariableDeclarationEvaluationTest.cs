using System;

using KSPCompiler.Domain;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.CompilerMessages.Extensions;
using KSPCompiler.Domain.Events;
using KSPCompiler.Domain.Events.Extensions;
using KSPCompiler.Interactors.Analysis.Semantics;
using KSPCompiler.Interactors.Tests.Commons;

using NUnit.Framework;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

[TestFixture]
public class AstArrayVariableDeclarationEvaluationTest
{
    [Test]
    public void DeclareWithInitializerTest()
    {
        const string name = "%variable";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

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

        var evaluator = new VariableDeclarationEvaluator( eventEmitter, symbols );
        var visitor = new MockDeclarationVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, declaration );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 0 ) );
        Assert.That( symbols.UserVariables.Count, Is.EqualTo( 1 ) );
    }

    [Test]
    public void CannotDeclareWithInvalidInitializerTest()
    {
        const string name = "%variable";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var symbols = MockUtility.CreateAggregateSymbolTable();

        // Variable can declare in init callback only
        var callbackAst = MockUtility.CreateCallbackDeclarationNode( "init" );

        // declare %variable
        var declaration = MockUtility.CreateVariableDeclarationNode( name );
        declaration.Parent = callbackAst;

        // [10] := (1, 2.0, "3")
        declaration.Initializer = new AstVariableInitializerNode( declaration )
        {
            ArrayInitializer = new AstArrayInitializerNode( declaration )
            {
                Size = new AstIntLiteralNode( 10 )
            }
        };
        declaration.Initializer.ArrayInitializer.Initializer.Expressions.Add(
            new AstIntLiteralNode( 1 )
        );
        declaration.Initializer.ArrayInitializer.Initializer.Expressions.Add(
            new AstRealLiteralNode( 2.0 )
        );
        declaration.Initializer.ArrayInitializer.Initializer.Expressions.Add(
            new AstStringLiteralNode( "3" )
        );

        var evaluator = new VariableDeclarationEvaluator( eventEmitter, symbols );
        var visitor = new MockDeclarationVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, declaration );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 2 ) );
        Assert.That( symbols.UserVariables.Count, Is.EqualTo( 1 ) );
    }

    [Test]
    public void CannotDeclareWithOutOfBoundInitializerTest()
    {
        const string name = "%variable";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var symbols = MockUtility.CreateAggregateSymbolTable();

        // Variable can declare in init callback only
        var callbackAst = MockUtility.CreateCallbackDeclarationNode( "init" );

        // declare %variable
        var declaration = MockUtility.CreateVariableDeclarationNode( name );
        declaration.Parent = callbackAst;

        // [5] := (1, 2, 3, 4, 5, 6, 7, 8, 9, 10)
        declaration.Initializer = new AstVariableInitializerNode( declaration )
        {
            ArrayInitializer = new AstArrayInitializerNode( declaration )
            {
                Size = new AstIntLiteralNode( 5 )
            }
        };

        for( var i = 0; i < 10; i++ )
        {
            declaration.Initializer.ArrayInitializer.Initializer.Expressions.Add(
                new AstIntLiteralNode( i + 1 )
            );
        }

        var evaluator = new VariableDeclarationEvaluator( eventEmitter, symbols );
        var visitor = new MockDeclarationVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, declaration );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 1 ) );
        Assert.That( symbols.UserVariables.Count, Is.EqualTo( 1 ) );
    }

    [Test]
    public void CannotDeclareWithConstTest()
    {
        const string name = "%variable";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var symbols = MockUtility.CreateAggregateSymbolTable();

        // Variable can declare in init callback only
        var callbackAst = MockUtility.CreateCallbackDeclarationNode( "init" );

        // declare const %variable <-- cannot declare const in array type
        var declaration = MockUtility.CreateVariableDeclarationNode( name );
        declaration.Parent   = callbackAst;
        declaration.Modifier = new AstModiferNode( declaration, "const" );

        var evaluator = new VariableDeclarationEvaluator( eventEmitter, symbols );
        var visitor = new MockDeclarationVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, declaration );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 1 ) );
        Assert.That( symbols.UserVariables.Count, Is.EqualTo( 0 ) );
    }

    [Test]
    public void CannotDeclareWithPrimitiveInitializer()
    {
        const string name = "%variable";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var symbols = MockUtility.CreateAggregateSymbolTable();

        // Variable can declare in init callback only
        var callbackAst = MockUtility.CreateCallbackDeclarationNode( "init" );

        // declare %variable := 0 <-- cannot declare with primitive initializer
        var declaration = MockUtility.CreateVariableDeclarationNode( name );
        declaration.Parent   = callbackAst;

        var initializer = new AstVariableInitializerNode( declaration );
        initializer.PrimitiveInitializer = new AstPrimitiveInitializerNode(
            initializer,
            new AstIntLiteralNode( 0 ),
            NullAstExpressionListNode.Instance
        );

        declaration.Initializer = initializer;

        var evaluator = new VariableDeclarationEvaluator( eventEmitter, symbols );
        var visitor = new MockDeclarationVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, declaration );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 1 ) );
        Assert.That( symbols.UserVariables.Count, Is.EqualTo( 1 ) );
    }

    [Test]
    public void DeclareWithoutInitializerTest()
    {
        const string name = "%variable";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var symbols = MockUtility.CreateAggregateSymbolTable();

        // Variable can declare in init callback only
        var callbackAst = MockUtility.CreateCallbackDeclarationNode( "init" );

        // declare %variable
        var declaration = MockUtility.CreateVariableDeclarationNode( name );
        declaration.Parent = callbackAst;

        // [10]
        declaration.Initializer = new AstVariableInitializerNode( declaration )
        {
            ArrayInitializer = new AstArrayInitializerNode( declaration )
            {
                Size = new AstIntLiteralNode( 10 )
            }
        };

        var evaluator = new VariableDeclarationEvaluator( eventEmitter, symbols );
        var visitor = new MockDeclarationVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, declaration );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 0 ) );
        Assert.That( symbols.UserVariables.Count, Is.EqualTo( 1 ) );
    }

    [TestCase( 0 )]
    [TestCase( -1 )]
    [TestCase( KspLanguageLimitations.MaxArraySize + 1 )]
    [TestCase( KspLanguageLimitations.MaxArraySize )]
    public void CannotDeclareWithInvalidSizeTest( int arraySize )
    {
        const string name = "%variable";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var symbols = MockUtility.CreateAggregateSymbolTable();

        // Variable can declare in init callback only
        var callbackAst = MockUtility.CreateCallbackDeclarationNode( "init" );

        // declare %variable
        var declaration = MockUtility.CreateVariableDeclarationNode( name );
        declaration.Parent = callbackAst;

        // [arraySize]
        declaration.Initializer = new AstVariableInitializerNode( declaration )
        {
            ArrayInitializer = new AstArrayInitializerNode( declaration )
            {
                Size = new AstIntLiteralNode( arraySize )
            }
        };

        var evaluator = new VariableDeclarationEvaluator( eventEmitter, symbols );
        var visitor = new MockDeclarationVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, declaration );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 1 ) );
        Assert.That( symbols.UserVariables.Count, Is.EqualTo( 1 ) );
    }
}
