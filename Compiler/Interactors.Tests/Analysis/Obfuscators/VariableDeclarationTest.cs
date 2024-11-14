using System;
using System.Collections.Generic;
using System.Text;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Extensions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Interactors.Analysis.Obfuscators.Extensions;
using KSPCompiler.UseCases.Analysis.Evaluations.Declarations;

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

        var obfuscatedTable = new ObfuscatedVariableTable( symbolTable.Variables, "v" );

        var node = new AstVariableDeclarationNode
        {
            Name = variableName
        };

        var evaluator = new VariableDeclarationEvaluator( output, symbolTable.Variables, symbolTable.UITypes, obfuscatedTable );
        var visitor = new MockVariableDeclarationVisitor( output );

        visitor.Inject( evaluator );
        visitor.Visit( node );

        Assert.AreEqual( $"declare {obfuscatedName}", output.ToString() );
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

        var obfuscatedTable = new ObfuscatedVariableTable( symbolTable.Variables, "v" );

        var node = new AstVariableDeclarationNode
        {
            Name     = variableName,
            Modifier = new AstModiferNode( [modifierText] )
        };

        var evaluator = new VariableDeclarationEvaluator( output, symbolTable.Variables, symbolTable.UITypes, obfuscatedTable );
        var visitor = new MockVariableDeclarationVisitor( output );

        visitor.Inject( evaluator );
        visitor.Visit( node );

        Assert.AreEqual( $"declare {modifierText} {obfuscatedName}", output.ToString() );
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

        var obfuscatedTable = new ObfuscatedVariableTable( symbolTable.Variables, "v" );

        var node = new AstVariableDeclarationNode
        {
            Name = variableName
        };

        var evaluator = new VariableDeclarationEvaluator( output, symbolTable.Variables, symbolTable.UITypes, obfuscatedTable );
        var visitor = new MockVariableDeclarationVisitor( output );

        visitor.Inject( evaluator );
        visitor.Visit( node );

        Assert.AreEqual( $"declare {variableName}", output.ToString() );
    }

    [Test]
    public void PrimitiveInitializerTest()
    {
        // declare $v0 := 1

        const string variableName = "$x";
        const string obfuscatedName = "$v0";
        const string expected = $"declare {obfuscatedName} := 1";

        var output = new StringBuilder();
        var symbolTable = MockUtility.CreateAggregateSymbolTable();

        symbolTable.Variables.Add( MockUtility.CreateIntVariable( variableName ) );

        var obfuscatedTable = new ObfuscatedVariableTable( symbolTable.Variables, "v" );

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
        const string expected = $"declare {obfuscatedName}[3] := (1, 2, 3)";

        var output = new StringBuilder();
        var symbolTable = MockUtility.CreateAggregateSymbolTable();

        var variable = MockUtility.CreateVariable( variableName, DataTypeFlag.TypeIntArray );

        symbolTable.Variables.Add( variable );

        var obfuscatedTable = new ObfuscatedVariableTable( symbolTable.Variables, "v" );

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
        const string expected = $"declare ui_label {obfuscatedName} := (1, 2)";

        var output = new StringBuilder();
        var symbolTable = MockUtility.CreateAggregateSymbolTable();

        var ui = MockUtility.CreateUILabel();
        symbolTable.UITypes.Add( ui );

        var variable = MockUtility.CreateVariable( variableName, DataTypeFlag.TypeInt );
        variable.Modifier = ModifierFlag.UI;
        variable.UIType   = ui;

        symbolTable.Variables.Add( variable );

        var obfuscatedTable = new ObfuscatedVariableTable( symbolTable.Variables, "v" );

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

}

public class MockVariableDeclarationVisitor : DefaultAstVisitor
{
    private StringBuilder OutputBuilder { get; }
    private IVariableDeclarationEvaluator Evaluator { get; set; } = new MockVariableDeclarationEvaluator();

    public MockVariableDeclarationVisitor( StringBuilder outputBuilder )
    {
        OutputBuilder = outputBuilder;
    }

    public void Inject( IVariableDeclarationEvaluator evaluator )
    {
        Evaluator = evaluator;
    }

    public override IAstNode Visit( AstVariableDeclarationNode node )
        => Evaluator.Evaluate( this, node );

    public override IAstNode Visit( AstIntLiteralNode node )
    {
        OutputBuilder.Append( node.Value );
        return node;
    }

    private class MockVariableDeclarationEvaluator : IVariableDeclarationEvaluator
    {
        public IAstNode Evaluate( IAstVisitor visitor, AstVariableDeclarationNode node )
            => throw new NotImplementedException();
    }
}


public class VariableDeclarationEvaluator : IVariableDeclarationEvaluator
{
    private StringBuilder OutputBuilder { get; }
    private IVariableSymbolTable Variables { get; }
    private ISymbolTable<UITypeSymbol> UITypeSymbols { get; }
    private IObfuscatedVariableTable ObfuscatedTable { get; }

    public VariableDeclarationEvaluator(
        StringBuilder outputBuilder,
        IVariableSymbolTable variables,
        ISymbolTable<UITypeSymbol> uiTypeSymbols,
        IObfuscatedVariableTable obfuscatedTable )
    {
        OutputBuilder   = outputBuilder;
        Variables       = variables;
        UITypeSymbols   = uiTypeSymbols;
        ObfuscatedTable = obfuscatedTable;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstVariableDeclarationNode node )
    {
        var name = ObfuscatedTable.GetObfuscatedByName( node.Name );

        if( !Variables.TrySearchByName( node.Name, out var variable ) )
        {
            throw new KeyNotFoundException( $"Variable not found: {node.Name} from variable symbol table" );
        }

        OutputBuilder.Append( "declare" );

        foreach( var modifier in node.Modifier.Values )
        {
            OutputBuilder.Append( $" {modifier}" );
        }

        if( variable.UIType != UITypeSymbol.Null )
        {
            OutputBuilder.Append( $" {variable.UIType.Name}" );
        }

        OutputBuilder.Append( $" {name}" );

        if( node.Initializer.IsNotNull() )
        {
            OutputInitializer( visitor, node, variable );
        }

        return node;
    }

    private void OutputInitializer( IAstVisitor visitor, AstVariableDeclarationNode node, VariableSymbol variable )
    {
        if( variable.Modifier.IsUI() )
        {
            OutputUIInitializer( visitor, node, variable );
            return;
        }
        else if( variable.DataType.IsArray() )
        {
            OutputArrayInitializer( visitor, node, variable );
            return;
        }

        OutputPrimitiveInitializer( visitor, node, variable );
    }

    #region Primitive Initializer

    private void OutputPrimitiveInitializer( IAstVisitor visitor, AstVariableDeclarationNode node, VariableSymbol variable )
    {
        OutputBuilder.Append( " := " );
        node.Initializer.PrimitiveInitializer.Accept( visitor );
    }

    #endregion ~Primitive Initializer


    #region Array Initializer

    private void OutputArrayInitializer( IAstVisitor visitor, AstVariableDeclarationNode node, VariableSymbol variable )
    {
        OutputArraySize( visitor, node, node.Initializer.ArrayInitializer, variable );
        OutputArrayElements( visitor, node, node.Initializer.ArrayInitializer, variable );
    }

    private void OutputArraySize( IAstVisitor visitor, AstVariableDeclarationNode node, AstArrayInitializerNode initializer, VariableSymbol variable )
    {
        OutputBuilder.Append( '[' );
        initializer.Size.Accept( visitor );
        OutputBuilder.Append( ']' );
    }

    private void OutputArrayElements( IAstVisitor visitor, AstVariableDeclarationNode node, AstArrayInitializerNode initializer, VariableSymbol variable )
    {
        OutputBuilder.Append( " := " );
        OutputBuilder.Append( '(' );

        OutputBuilder.AppendExpressionList( visitor, initializer.Initializer );

        OutputBuilder.Append( ')' );
    }

    #endregion ~Array Initializer

    #region UI Initializer

    private void OutputUIInitializer( IAstVisitor visitor, AstVariableDeclarationNode node, VariableSymbol variable )
    {
        if( variable.DataType.IsArray() )
        {
            OutputArrayBasedUIInitializer( visitor, node, node.Initializer.ArrayInitializer, variable );
        }
        else
        {
            OutputPrimitiveBasedUIInitializer( visitor, node, node.Initializer.PrimitiveInitializer.UIInitializer, variable );
        }
    }

    private void OutputArrayBasedUIInitializer( IAstVisitor visitor, AstVariableDeclarationNode node, AstArrayInitializerNode initializer, VariableSymbol variable )
    {
        OutputArraySize( visitor, node, initializer, variable );
        OutputUIArguments( visitor, node, initializer.Initializer, variable.UIType );
    }

    private void OutputPrimitiveBasedUIInitializer( IAstVisitor visitor, AstVariableDeclarationNode node, AstExpressionListNode initializer, VariableSymbol variable )
    {
        OutputUIArguments( visitor, node, initializer, variable.UIType );
    }

    private void OutputUIArguments( IAstVisitor visitor, AstVariableDeclarationNode node, AstExpressionListNode expressionList, UITypeSymbol uiType )
    {
        OutputBuilder.Append( " := " );
        OutputBuilder.Append( '(' );

        OutputBuilder.AppendExpressionList( visitor, expressionList );

        OutputBuilder.Append( ')' );
    }

    #endregion ~UI Initializer
}




public interface IObfuscatedTable<in TSymbol> where TSymbol : SymbolBase
{
    bool TryGetObfuscatedByName( string original, out string result );
    string GetObfuscatedByName( string original );
}

public interface IObfuscatedVariableTable : IObfuscatedTable<VariableSymbol> {}
public interface IObfuscatedUserFunctionTable : IObfuscatedTable<UserFunctionSymbol> {}

public interface IObfuscateFormatter
{
    string Format( string original, string prefix, UniqueSymbolIndex index );
}

public sealed class DefaultObfuscateFormatter : IObfuscateFormatter
{
    public string Format( string original, string prefix, UniqueSymbolIndex index )
        => $"{prefix}{index.Value}";
}

abstract public class ObfuscatedSymbolTable<TSymbol> : IObfuscatedTable<TSymbol> where TSymbol : SymbolBase
{
    // <original name, obfuscated name>
    private Dictionary<string, string> ObfuscatedTable { get; } = new();

    private string Prefix { get; }

    private IObfuscateFormatter Formatter { get; }

    protected ObfuscatedSymbolTable( ISymbolTable<TSymbol> source, string prefix )
        : this( source, prefix, new DefaultObfuscateFormatter() ) {}

    protected ObfuscatedSymbolTable( ISymbolTable<TSymbol> source, string prefix, IObfuscateFormatter formatter )
    {
        Prefix    = prefix;
        Formatter = formatter;

        var generator = new UniqueSymbolIndexGenerator();
        Obfuscate( source, generator );
    }

    private void Obfuscate( ISymbolTable<TSymbol> source, UniqueSymbolIndexGenerator generator )
    {
        foreach( var (name, symbol) in source.Table )
        {
            Obfuscate( name, symbol, generator );
        }
    }

    private void Obfuscate( string name, TSymbol symbol, UniqueSymbolIndexGenerator generator )
    {
        var typePrefix = string.Empty;

        if( symbol.BuiltIn )
        {
            return;
        }

        if( KspRegExpConstants.TypePrefix.IsMatch( name ) )
        {
            typePrefix = name[ 0 ].ToString();
        }

        var obfuscatedName = $"{typePrefix}{Formatter.Format( name, Prefix, generator.Next() )}";
        ObfuscatedTable.TryAdd( name, obfuscatedName );
    }

    public bool TryGetObfuscatedByName( string original, out string result )
    {
        result = string.Empty;

        if( !ObfuscatedTable.TryGetValue( original, out var obfuscated ) )
        {
            return false;
        }

        result = obfuscated;
        return true;
    }

    public string GetObfuscatedByName( string original )
        => !TryGetObfuscatedByName( original, out var result ) ? original : result;
}

public class ObfuscatedVariableTable : ObfuscatedSymbolTable<VariableSymbol>, IObfuscatedVariableTable
{
    public ObfuscatedVariableTable( IVariableSymbolTable source, string prefix )
        : base( source, prefix ) {}

    public ObfuscatedVariableTable( IVariableSymbolTable source, string prefix, IObfuscateFormatter formatter )
        : base( source, prefix, formatter ) {}
}
