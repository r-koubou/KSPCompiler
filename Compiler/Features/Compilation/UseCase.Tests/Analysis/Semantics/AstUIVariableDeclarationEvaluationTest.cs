using System;

using KSPCompiler.Features.Compilation.Domain.Messages;
using KSPCompiler.Features.Compilation.Domain.Messages.Extensions;
using KSPCompiler.Features.Compilation.Gateways.EventEmitting;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics;
using KSPCompiler.Features.SymbolManagement.UseCase.Tests.Commons;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Statements;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;
using KSPCompiler.Shared.EventEmitting.Extensions;

using NUnit.Framework;

namespace KSPCompiler.Features.SymbolManagement.UseCase.Tests.Analysis.Semantics;

[TestFixture]
public class AstUIVariableDeclarationEvaluationTest
{
    [Test]
    public void DeclarePrimitiveBaseTest()
    {
        const string name = "$variable";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var symbols = new AggregateSymbolTable();

        // Variable can declare in init callback only
        var callbackAst = MockUtility.CreateCallbackDeclarationNode( "init" );

        // define UI type : ksp ui_label
        var uiLabelType = MockUtility.CreateUILabel();
        symbols.UITypes.Add( uiLabelType );

        // declare $variable
        var declaration = MockUtility.CreateVariableDeclarationNode( name );
        declaration.Parent   = callbackAst;
        declaration.Modifier = new AstModiferNode( declaration, uiLabelType.Name );

        // (1, 2)
        declaration.Initializer = new AstVariableInitializerNode( declaration )
        {
            PrimitiveInitializer = new AstPrimitiveInitializerNode
            {
                Parent = declaration
            }
        };
        declaration.Initializer.PrimitiveInitializer.UIInitializer.Expressions.AddRange( new AstExpressionNode[]
        {
            new AstIntLiteralNode( 1 ),
            new AstIntLiteralNode( 2 )
        });

        var evaluator = new VariableDeclarationEvaluator( eventEmitter, symbols );
        var visitor = new MockDeclarationVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, declaration );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 0 ) );
        Assert.That( symbols.UserVariables.Count, Is.EqualTo( 1 ) );
    }

    [TestCase( 2, 1, 2, 10 )]
    [TestCase( 10, 2, 2, 10 )]
    public void DeclareArrayBaseByUITableTest(int arraySize, int gridWidth, int gridHeight, int range )
    {
        // declare ui_table %table[arraySize] (gridWidth, gridHeight, range)

        const string name = "%table";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var symbols = new AggregateSymbolTable();

        // Variable can declare in init callback only
        var callbackAst = MockUtility.CreateCallbackDeclarationNode( "init" );

        // define UI type : ksp ui_table
        var uiLabelType = MockUtility.CreateUITable();
        symbols.UITypes.Add( uiLabelType );

        // declare %variable[arraySize]
        var declaration = MockUtility.CreateVariableDeclarationNode( name );
        declaration.Parent   = callbackAst;
        declaration.Modifier = new AstModiferNode( declaration, uiLabelType.Name );

        // [arraySize] (gridWidth, gridHeight, range)
        declaration.Initializer = new AstVariableInitializerNode( declaration )
        {
            ArrayInitializer = new AstArrayInitializerNode
            {
                Parent = declaration,
                Size   = new AstIntLiteralNode( arraySize )
            },
            PrimitiveInitializer = new AstPrimitiveInitializerNode
            {
                Parent = declaration
            }
        };

        declaration.Initializer.ArrayInitializer.Initializer.Expressions.AddRange( new AstExpressionNode[]
        {
            new AstIntLiteralNode( gridWidth ),
            new AstIntLiteralNode( gridHeight ),
            new AstIntLiteralNode( range )
        });

        var evaluator = new VariableDeclarationEvaluator( eventEmitter, symbols );
        var visitor = new MockDeclarationVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, declaration );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 0 ) );
        Assert.That( symbols.UserVariables.Count, Is.EqualTo( 1 ) );
    }

    [Test]
    public void CannotDeclareNoRegisteredUITest()
    {
        const string name = "$variable";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var symbols = new AggregateSymbolTable();

        // Variable can declare in init callback only
        var callbackAst = MockUtility.CreateCallbackDeclarationNode( "init" );

        // declare $variable <-- ui_unknown is not registered symbol table
        var declaration = MockUtility.CreateVariableDeclarationNode( name );
        declaration.Parent   = callbackAst;
        declaration.Modifier = new AstModiferNode( declaration, "ui_unknown" );

        var evaluator = new VariableDeclarationEvaluator( eventEmitter, symbols );
        var visitor = new MockDeclarationVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, declaration );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 1 ) );
        Assert.That( symbols.UserVariables.Count, Is.EqualTo( 1 ) );
    }

    [Test]
    public void CannotDeclareNoParameterCountTest()
    {
        const string name = "$variable";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var symbols = new AggregateSymbolTable();

        // Variable can declare in init callback only
        var callbackAst = MockUtility.CreateCallbackDeclarationNode( "init" );

        // define UI type : ksp ui_label
        var uiLabelType = MockUtility.CreateUILabel();
        symbols.UITypes.Add( uiLabelType );

        // declare $variable
        var declaration = MockUtility.CreateVariableDeclarationNode( name );
        declaration.Parent   = callbackAst;
        declaration.Modifier = new AstModiferNode( declaration, uiLabelType.Name );

        // Non parameter <-- invalid parameter count (expected 2 parameters)
        declaration.Initializer = new AstVariableInitializerNode( declaration )
        {
            PrimitiveInitializer = new AstPrimitiveInitializerNode
            {
                Parent = declaration
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

    [Test]
    public void CannotDeclareIncompatibleParameterCountTest()
    {
        const string name = "$variable";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var symbols = new AggregateSymbolTable();

        // Variable can declare in init callback only
        var callbackAst = MockUtility.CreateCallbackDeclarationNode( "init" );

        // define UI type : ksp ui_label
        var uiLabelType = MockUtility.CreateUILabel();
        symbols.UITypes.Add( uiLabelType );

        // declare $variable
        var declaration = MockUtility.CreateVariableDeclarationNode( name );
        declaration.Parent   = callbackAst;
        declaration.Modifier = new AstModiferNode( declaration, uiLabelType.Name );

        // (1) <-- invalid parameter count (expected 2 parameters)
        declaration.Initializer = new AstVariableInitializerNode( declaration )
        {
            PrimitiveInitializer = new AstPrimitiveInitializerNode
            {
                Parent = declaration
            }
        };
        declaration.Initializer.PrimitiveInitializer.UIInitializer.Expressions.AddRange( new AstExpressionNode[]
        {
            new AstIntLiteralNode( 1 )
        });

        var evaluator = new VariableDeclarationEvaluator( eventEmitter, symbols );
        var visitor = new MockDeclarationVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, declaration );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 1 ) );
        Assert.That( symbols.UserVariables.Count, Is.EqualTo( 1 ) );
    }

    [Test]
    public void CannotDeclareIncompatibleParameterTypeTest()
    {
        const string name = "$variable";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var symbols = new AggregateSymbolTable();

        // Variable can declare in init callback only
        var callbackAst = MockUtility.CreateCallbackDeclarationNode( "init" );

        // define UI type : ksp ui_label
        var uiLabelType = MockUtility.CreateUILabel();
        symbols.UITypes.Add( uiLabelType );

        // declare $variable
        var declaration = MockUtility.CreateVariableDeclarationNode( name );
        declaration.Parent   = callbackAst;
        declaration.Modifier = new AstModiferNode( declaration, uiLabelType.Name );

        // (1, 2.0) <-- invalid parameter type (expected 2 integer parameters)
        declaration.Initializer = new AstVariableInitializerNode( declaration )
        {
            PrimitiveInitializer = new AstPrimitiveInitializerNode
            {
                Parent = declaration
            }
        };
        declaration.Initializer.PrimitiveInitializer.UIInitializer.Expressions.AddRange( new AstExpressionNode[]
        {
            new AstIntLiteralNode( 1 ),
            new AstRealLiteralNode( 2.0 )
        });

        var evaluator = new VariableDeclarationEvaluator( eventEmitter, symbols );
        var visitor = new MockDeclarationVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, declaration );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 1 ) );
        Assert.That( symbols.UserVariables.Count, Is.EqualTo( 1 ) );
    }

    [Test]
    public void CannotDeclareNoConstantParameterTest()
    {
        const string name = "$variable";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var symbols = new AggregateSymbolTable();

        // Variable can declare in init callback only
        var callbackAst = MockUtility.CreateCallbackDeclarationNode( "init" );

        // define UI type : ksp ui_label
        var uiLabelType = MockUtility.CreateUILabel();
        symbols.UITypes.Add( uiLabelType );

        // no constant parameter
        symbols.UserVariables.Add( MockUtility.CreateIntVariable( "$arg" ) );
        var argNode = MockUtility.CreateSymbolNode( "$arg", DataTypeFlag.TypeInt );

        // declare $variable
        var declaration = MockUtility.CreateVariableDeclarationNode( name );
        declaration.Parent   = callbackAst;
        declaration.Modifier = new AstModiferNode( declaration, uiLabelType.Name );

        // (1, $arg) <-- $arg is not constant
        declaration.Initializer = new AstVariableInitializerNode( declaration )
        {
            PrimitiveInitializer = new AstPrimitiveInitializerNode
            {
                Parent = declaration
            }
        };
        declaration.Initializer.PrimitiveInitializer.UIInitializer.Expressions.AddRange( new AstExpressionNode[]
        {
            new AstIntLiteralNode( 1 ),
            argNode
        });

        var evaluator = new VariableDeclarationEvaluator( eventEmitter, symbols );
        var visitor = new MockDeclarationVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, declaration );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 1 ) );
        Assert.That( symbols.UserVariables.Count, Is.EqualTo( 2 ) );
    }
}
