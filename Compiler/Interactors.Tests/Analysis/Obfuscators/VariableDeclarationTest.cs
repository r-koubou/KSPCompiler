using System.Text;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.Symbols;
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

        var variable = MockUtility.CreateIntVariable( variableName );
        variable.State = SymbolState.Loaded;

        symbolTable.UserVariables.Add( variable );

        var obfuscatedTable = new ObfuscatedVariableSymbolTable( symbolTable.UserVariables, "v" );

        var node = new AstVariableDeclarationNode
        {
            Name = variableName
        };

        var evaluator = new VariableDeclarationEvaluator( output, symbolTable.UserVariables, symbolTable.UITypes, obfuscatedTable );
        var visitor = new MockVariableDeclarationVisitor( output );

        visitor.Inject( evaluator );
        visitor.Visit( node );

        Assert.That( output.ToString(), Is.EqualTo( $"declare {obfuscatedName}{ObfuscatorConstants.NewLine}" ) );
    }

    [Test]
    public void ShrinkTest()
    {
        const string variableName = "$x";

        var output = new StringBuilder();
        var symbolTable = MockUtility.CreateAggregateSymbolTable();

        var variable = MockUtility.CreateIntVariable( variableName );

        // 初期値代入のみで参照されない状況を作成
        variable.State = SymbolState.Initialized;

        symbolTable.UserVariables.Add( variable );

        var obfuscatedTable = new ObfuscatedVariableSymbolTable( symbolTable.UserVariables, "v" );

        var node = new AstVariableDeclarationNode
        {
            Name = variableName
        };

        var evaluator = new VariableDeclarationEvaluator( output, symbolTable.UserVariables, symbolTable.UITypes, obfuscatedTable );
        var visitor = new MockVariableDeclarationVisitor( output );

        visitor.Inject( evaluator );
        visitor.Visit( node );

        Assert.That( output.ToString(), Is.EqualTo( string.Empty ) );
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
        variable.State    = SymbolState.Loaded;

        symbolTable.UserVariables.Add( variable );

        var obfuscatedTable = new ObfuscatedVariableSymbolTable( symbolTable.UserVariables, "v" );

        var node = new AstVariableDeclarationNode
        {
            Name     = variableName,
            Modifier = new AstModiferNode( [modifierText] )
        };

        var evaluator = new VariableDeclarationEvaluator( output, symbolTable.UserVariables, symbolTable.UITypes, obfuscatedTable );
        var visitor = new MockVariableDeclarationVisitor( output );

        visitor.Inject( evaluator );
        visitor.Visit( node );

        Assert.That( output.ToString(), Is.EqualTo( $"declare {modifierText} {obfuscatedName}{ObfuscatorConstants.NewLine}" ) );
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
        var variable = MockUtility.CreateIntVariable( variableName );

        variable.State = SymbolState.Loaded;
        symbolTable.UserVariables.Add( variable );

        var obfuscatedTable = new ObfuscatedVariableSymbolTable( symbolTable.UserVariables, "v" );

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

        var evaluator = new VariableDeclarationEvaluator( output, symbolTable.UserVariables, symbolTable.UITypes, obfuscatedTable );
        var visitor = new MockVariableDeclarationVisitor( output );

        visitor.Inject( evaluator );
        visitor.Visit( node );

        Assert.That( output.ToString(), Is.EqualTo( expected ) );
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
        variable.State = SymbolState.Loaded;

        symbolTable.UserVariables.Add( variable );

        var obfuscatedTable = new ObfuscatedVariableSymbolTable( symbolTable.UserVariables, "v" );

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

        var evaluator = new VariableDeclarationEvaluator( output, symbolTable.UserVariables, symbolTable.UITypes, obfuscatedTable );
        var visitor = new MockVariableDeclarationVisitor( output );

        visitor.Inject( evaluator );
        visitor.Visit( node );

        Assert.That( output.ToString(), Is.EqualTo( expected ) );
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
        variable.State    = SymbolState.Loaded;

        symbolTable.UserVariables.Add( variable );

        var obfuscatedTable = new ObfuscatedVariableSymbolTable( symbolTable.UserVariables, "v" );

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

        var evaluator = new VariableDeclarationEvaluator( output, symbolTable.UserVariables, symbolTable.UITypes, obfuscatedTable );
        var visitor = new MockVariableDeclarationVisitor( output );

        visitor.Inject( evaluator );
        visitor.Visit( node );

        Assert.That( output.ToString(), Is.EqualTo( expected ) );
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
        variable.State    = SymbolState.Loaded;

        symbolTable.UserVariables.Add( variable );

        var obfuscatedTable = new ObfuscatedVariableSymbolTable( symbolTable.UserVariables, "v" );

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

        var evaluator = new VariableDeclarationEvaluator( output, symbolTable.UserVariables, symbolTable.UITypes, obfuscatedTable );
        var visitor = new MockVariableDeclarationVisitor( output );

        visitor.Inject( evaluator );
        visitor.Visit( node );

        Assert.That( output.ToString(), Is.EqualTo( expected ) );
    }
}
