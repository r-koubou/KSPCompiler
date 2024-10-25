using System;

using KSPCompiler.Domain.Ast.Analyzers.Semantics;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols.MetaData;

using NUnit.Framework;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

[TestFixture]
public class AstUIVariableDeclarationEvaluationTest
{
    [Test]
    public void DeclareLabelTest()
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
        declaration.Modifier = uiLabelType.Name;

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

        Assert.AreEqual( 0, compilerMessageManger.Count( CompilerMessageLevel.Error ) );
        Assert.AreEqual( 1, symbols.Variables.Count );
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
        declaration.Modifier = uiLabelType.Name;

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

        Assert.AreEqual( 1, compilerMessageManger.Count( CompilerMessageLevel.Error ) );
        Assert.AreEqual( 0, symbols.Variables.Count );
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
        declaration.Modifier = uiLabelType.Name;

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

        Assert.AreEqual( 1, compilerMessageManger.Count( CompilerMessageLevel.Error ) );
        Assert.AreEqual( 0, symbols.Variables.Count );
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
        declaration.Modifier = uiLabelType.Name;

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

        Assert.AreEqual( 1, compilerMessageManger.Count( CompilerMessageLevel.Error ) );
        Assert.AreEqual( 1, symbols.Variables.Count );
    }
}
