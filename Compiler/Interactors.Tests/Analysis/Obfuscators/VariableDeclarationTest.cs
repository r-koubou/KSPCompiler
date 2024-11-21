using System.Text;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Interactors.Analysis.Obfuscators;
using KSPCompiler.Interactors.Analysis.Obfuscators.Extensions;

using NUnit.Framework;

namespace KSPCompiler.Interactors.Tests.Analysis.Obfuscators;

[TestFixture]
public class VariableDeclarationTest
{
    [Test]
    public void Test()
    {
        const string variableName = "$x";
        const string obfuscatedName = "$v0";

        var output = new StringBuilder();
        var symbolTable = MockUtility.CreateAggregateSymbolTable();

        symbolTable.Variables.Add( MockUtility.CreateIntVariable( variableName ) );

        var obfuscatedTable = new ObfuscatedVariableSymbolTable( symbolTable.Variables, "v" );

        var node = new AstVariableDeclarationNode
        {
            Name = variableName
        };

        var evaluator = new VariableDeclarationEvaluator( output, symbolTable.Variables, symbolTable.UITypes, obfuscatedTable );
        var visitor = new MockVariableDeclarationVisitor( output );

        visitor.Inject( evaluator );
        visitor.Visit( node );

        Assert.AreEqual( $"declare {obfuscatedName}{ObfuscatorConstants.NewLine}", output.ToString() );
    }

    [TestCase( ModifierFlag.Const, "const" )]
    [TestCase( ModifierFlag.Polyphonic, "polyphonic" )]
    public void ModifierTest( ModifierFlag modifier, string modifierText )
    {
        const string variableName = "$x";
        const string obfuscatedName = "$v0";

        var output = new StringBuilder();
        var symbolTable = MockUtility.CreateAggregateSymbolTable();

        var variable = MockUtility.CreateIntVariable( variableName );
        variable.Modifier = modifier;

        symbolTable.Variables.Add( variable );

        var obfuscatedTable = new ObfuscatedVariableSymbolTable( symbolTable.Variables, "v" );

        var node = new AstVariableDeclarationNode
        {
            Name     = variableName,
            Modifier = new AstModiferNode( [modifierText] )
        };

        var evaluator = new VariableDeclarationEvaluator( output, symbolTable.Variables, symbolTable.UITypes, obfuscatedTable );
        var visitor = new MockVariableDeclarationVisitor( output );

        visitor.Inject( evaluator );
        visitor.Visit( node );

        Assert.AreEqual( $"declare {modifierText} {obfuscatedName}{ObfuscatorConstants.NewLine}", output.ToString() );
    }

    [Test]
    public void CannotObfuscateToBuiltInVariableTest()
    {
        const string variableName = "$ENGINE_PAR_TEST";

        var output = new StringBuilder();
        var symbolTable = MockUtility.CreateAggregateSymbolTable();

        var variable = MockUtility.CreateIntVariable( variableName );
        variable.BuiltIn = true;

        symbolTable.Variables.Add( variable );

        var obfuscatedTable = new ObfuscatedVariableSymbolTable( symbolTable.Variables, "v" );

        var node = new AstVariableDeclarationNode
        {
            Name = variableName
        };

        var evaluator = new VariableDeclarationEvaluator( output, symbolTable.Variables, symbolTable.UITypes, obfuscatedTable );
        var visitor = new MockVariableDeclarationVisitor( output );

        visitor.Inject( evaluator );
        visitor.Visit( node );

        Assert.AreEqual( $"declare {variableName}{ObfuscatorConstants.NewLine}", output.ToString() );
    }

    [Test]
    public void PrimitiveInitializerTest()
    {
        // declare $v0 := 1

        const string variableName = "$x";
        const string obfuscatedName = "$v0";
        var expected = new StringBuilder()
                      .Append( $"declare {obfuscatedName} := 1" )
                      .NewLine().ToString();

        var output = new StringBuilder();
        var symbolTable = MockUtility.CreateAggregateSymbolTable();

        symbolTable.Variables.Add( MockUtility.CreateIntVariable( variableName ) );

        var obfuscatedTable = new ObfuscatedVariableSymbolTable( symbolTable.Variables, "v" );

        var node = new AstVariableDeclarationNode
        {
            Name = variableName,
            Initializer = new AstVariableInitializerNode
            {
                PrimitiveInitializer = new AstPrimitiveInitializerNode
                {
                    Expression = new AstIntLiteralNode( 1 )
                }
            }
        };

        var evaluator = new VariableDeclarationEvaluator( output, symbolTable.Variables, symbolTable.UITypes, obfuscatedTable );
        var visitor = new MockVariableDeclarationVisitor( output );

        visitor.Inject( evaluator );
        visitor.Visit( node );

        Assert.AreEqual( expected, output.ToString() );
    }

    [Test]
    public void ArrayInitializerTest()
    {
        // declare %v0[3] := (1, 2, 3)

        const string variableName = "%x";
        const string obfuscatedName = "%v0";
        var expected = new StringBuilder()
                      .Append( $"declare {obfuscatedName}[3] := (1, 2, 3)" )
                      .NewLine().ToString();

        var output = new StringBuilder();
        var symbolTable = MockUtility.CreateAggregateSymbolTable();

        var variable = MockUtility.CreateVariable( variableName, DataTypeFlag.TypeIntArray );

        symbolTable.Variables.Add( variable );

        var obfuscatedTable = new ObfuscatedVariableSymbolTable( symbolTable.Variables, "v" );

        var initializer = new AstArrayInitializerNode
        {
            Size = new AstIntLiteralNode( 3 )
        };
        initializer.Initializer.Expressions.Add( new AstIntLiteralNode( 1 ) );
        initializer.Initializer.Expressions.Add( new AstIntLiteralNode( 2 ) );
        initializer.Initializer.Expressions.Add( new AstIntLiteralNode( 3 ) );

        var node = new AstVariableDeclarationNode
        {
            Name = variableName,
            Initializer = new AstVariableInitializerNode
            {
                ArrayInitializer = initializer
            }
        };

        var evaluator = new VariableDeclarationEvaluator( output, symbolTable.Variables, symbolTable.UITypes, obfuscatedTable );
        var visitor = new MockVariableDeclarationVisitor( output );

        visitor.Inject( evaluator );
        visitor.Visit( node );

        Assert.AreEqual( expected, output.ToString() );
    }

    [Test]
    public void UIInitializerTest()
    {
        // declare ui_label $v0(1, 2)

        const string variableName = "$x";
        const string obfuscatedName = "$v0";
        var expected = new StringBuilder()
                      .Append( $"declare ui_label {obfuscatedName} := (1, 2)" )
                      .NewLine().ToString();

        var output = new StringBuilder();
        var symbolTable = MockUtility.CreateAggregateSymbolTable();

        var ui = MockUtility.CreateUILabel();
        symbolTable.UITypes.Add( ui );

        var variable = MockUtility.CreateVariable( variableName, DataTypeFlag.TypeInt );
        variable.Modifier = ModifierFlag.UI;
        variable.UIType   = ui;

        symbolTable.Variables.Add( variable );

        var obfuscatedTable = new ObfuscatedVariableSymbolTable( symbolTable.Variables, "v" );

        var uiInitializer = new AstExpressionListNode();
        uiInitializer.Expressions.Add( new AstIntLiteralNode( 1 ) );
        uiInitializer.Expressions.Add( new AstIntLiteralNode( 2 ) );

        var node = new AstVariableDeclarationNode
        {
            Name = variableName,
            Initializer = new AstVariableInitializerNode
            {
                PrimitiveInitializer = new AstPrimitiveInitializerNode(
                    NullAstNode.Instance,
                    NullAstExpressionNode.Instance,
                    uiInitializer
                )
            }
        };

        var evaluator = new VariableDeclarationEvaluator( output, symbolTable.Variables, symbolTable.UITypes, obfuscatedTable );
        var visitor = new MockVariableDeclarationVisitor( output );

        visitor.Inject( evaluator );
        visitor.Visit( node );

        Assert.AreEqual( expected, output.ToString() );
    }

    [Test]
    public void ArrayUIInitializerTest()
    {
        // declare ui_table $v0[2] := (1, 2)

        const string variableName = "%x";
        const string obfuscatedName = "%v0";
        var expected = new StringBuilder()
                      .Append( $"declare ui_table {obfuscatedName}[2] := (1, 2)" )
                      .NewLine().ToString();

        var output = new StringBuilder();
        var symbolTable = MockUtility.CreateAggregateSymbolTable();

        var ui = MockUtility.CreateUITable();
        symbolTable.UITypes.Add( ui );

        var variable = MockUtility.CreateVariable( variableName, DataTypeFlag.TypeIntArray );
        variable.Modifier = ModifierFlag.UI;
        variable.UIType   = ui;

        symbolTable.Variables.Add( variable );

        var obfuscatedTable = new ObfuscatedVariableSymbolTable( symbolTable.Variables, "v" );

        var uiInitializer = new AstExpressionListNode();
        uiInitializer.Expressions.Add( new AstIntLiteralNode( 1 ) );
        uiInitializer.Expressions.Add( new AstIntLiteralNode( 2 ) );

        var arraySize = new AstArrayInitializerNode
        {
            Size = new AstIntLiteralNode( 2 ),
            Initializer = uiInitializer
        };

        var node = new AstVariableDeclarationNode
        {
            Name = variableName,
            Initializer = new AstVariableInitializerNode
            {
                ArrayInitializer = arraySize
            }
        };

        var evaluator = new VariableDeclarationEvaluator( output, symbolTable.Variables, symbolTable.UITypes, obfuscatedTable );
        var visitor = new MockVariableDeclarationVisitor( output );

        visitor.Inject( evaluator );
        visitor.Visit( node );

        Assert.AreEqual( expected, output.ToString() );
    }
}
