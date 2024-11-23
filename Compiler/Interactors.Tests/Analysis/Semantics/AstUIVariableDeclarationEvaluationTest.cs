using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Interactors.Analysis.Semantics;

using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

[TestFixture]
public class AstUIVariableDeclarationEvaluationTest
{
    [Test]
    public void DeclarePrimitiveBaseTest()
    {
        const string name = "$variable";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var symbols = MockUtility.CreateAggregateSymbolTable();

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

        var evaluator = new VariableDeclarationEvaluator( compilerMessageManger, symbols.Variables, symbols.UITypes );
        var visitor = new MockDeclarationVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, declaration );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 0 ) );
        Assert.That( symbols.Variables.Count, Is.EqualTo( 1 ) );
    }

    [Test]
    public void DeclareArrayBaseTest()
    {
        // declare ui_table %table[10] (2, 2, 10)

        const string name = "%table";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var symbols = MockUtility.CreateAggregateSymbolTable();

        // Variable can declare in init callback only
        var callbackAst = MockUtility.CreateCallbackDeclarationNode( "init" );

        // define UI type : ksp ui_table
        var uiLabelType = MockUtility.CreateUITable();
        symbols.UITypes.Add( uiLabelType );

        // declare %variable[10]
        var declaration = MockUtility.CreateVariableDeclarationNode( name );
        declaration.Parent   = callbackAst;
        declaration.Modifier = new AstModiferNode( declaration, uiLabelType.Name );

        // [10] (2, 2, 10)
        declaration.Initializer = new AstVariableInitializerNode( declaration )
        {
            ArrayInitializer = new AstArrayInitializerNode
            {
                Parent = declaration,
                Size   = new AstIntLiteralNode( 10 )
            },
            PrimitiveInitializer = new AstPrimitiveInitializerNode
            {
                Parent = declaration
            }
        };
        declaration.Initializer.ArrayInitializer.Initializer.Expressions.AddRange( new AstExpressionNode[]
        {
            new AstIntLiteralNode( 2 ),
            new AstIntLiteralNode( 2 ),
            new AstIntLiteralNode( 10 )
        });

        var evaluator = new VariableDeclarationEvaluator( compilerMessageManger, symbols.Variables, symbols.UITypes );
        var visitor = new MockDeclarationVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, declaration );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 0 ) );
        Assert.That( symbols.Variables.Count, Is.EqualTo( 1 ) );
    }

    [Test]
    public void CannotDeclareNoRegisteredUITest()
    {
        const string name = "$variable";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var symbols = MockUtility.CreateAggregateSymbolTable();

        // Variable can declare in init callback only
        var callbackAst = MockUtility.CreateCallbackDeclarationNode( "init" );

        // declare $variable <-- ui_unknown is not registered symbol table
        var declaration = MockUtility.CreateVariableDeclarationNode( name );
        declaration.Parent   = callbackAst;
        declaration.Modifier = new AstModiferNode( declaration, "ui_unknown" );

        var evaluator = new VariableDeclarationEvaluator( compilerMessageManger, symbols.Variables, symbols.UITypes );
        var visitor = new MockDeclarationVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, declaration );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 1 ) );
        Assert.That( symbols.Variables.Count, Is.EqualTo( 0 ) );
    }

    [Test]
    public void CannotDeclareNoParameterCountTest()
    {
        const string name = "$variable";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var symbols = MockUtility.CreateAggregateSymbolTable();

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

        var evaluator = new VariableDeclarationEvaluator( compilerMessageManger, symbols.Variables, symbols.UITypes );
        var visitor = new MockDeclarationVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, declaration );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 1 ) );
        Assert.That( symbols.Variables.Count, Is.EqualTo( 0 ) );
    }

    [Test]
    public void CannotDeclareIncompatibleParameterCountTest()
    {
        const string name = "$variable";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var symbols = MockUtility.CreateAggregateSymbolTable();

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

        var evaluator = new VariableDeclarationEvaluator( compilerMessageManger, symbols.Variables, symbols.UITypes );
        var visitor = new MockDeclarationVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, declaration );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 1 ) );
        Assert.That( symbols.Variables.Count, Is.EqualTo( 0 ) );
    }

    [Test]
    public void CannotDeclareIncompatibleParameterTypeTest()
    {
        const string name = "$variable";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var symbols = MockUtility.CreateAggregateSymbolTable();

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

        var evaluator = new VariableDeclarationEvaluator( compilerMessageManger, symbols.Variables, symbols.UITypes );
        var visitor = new MockDeclarationVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, declaration );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 1 ) );
        Assert.That( symbols.Variables.Count, Is.EqualTo( 0 ) );
    }

    [Test]
    public void CannotDeclareNoConstantParameterTest()
    {
        const string name = "$variable";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var symbols = MockUtility.CreateAggregateSymbolTable();

        // Variable can declare in init callback only
        var callbackAst = MockUtility.CreateCallbackDeclarationNode( "init" );

        // define UI type : ksp ui_label
        var uiLabelType = MockUtility.CreateUILabel();
        symbols.UITypes.Add( uiLabelType );

        // no constant parameter
        symbols.Variables.Add( MockUtility.CreateIntVariable( "$arg" ) );
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

        var evaluator = new VariableDeclarationEvaluator( compilerMessageManger, symbols.Variables, symbols.UITypes );
        var visitor = new MockDeclarationVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, declaration );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 1 ) );
        Assert.That( symbols.Variables.Count, Is.EqualTo( 1 ) );
    }
}
