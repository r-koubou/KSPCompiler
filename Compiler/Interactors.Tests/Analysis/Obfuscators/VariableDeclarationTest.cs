using System;
using System.Collections.Generic;
using System.Text;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Extensions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.UseCases.Analysis.Evaluations.Declarations;

using NUnit.Framework;

namespace KSPCompiler.Interactors.Tests.Analysis.Obfuscators;

[TestFixture]
public class VariableDeclarationTest
{
}

public class MockVariableDeclarationVisitor : DefaultAstVisitor
{
    private IVariableDeclarationEvaluator Evaluator { get; set; } = new MockVariableDeclarationEvaluator();

    public void Inject( IVariableDeclarationEvaluator evaluator )
    {
        Evaluator = evaluator;
    }

    public override IAstNode Visit( AstVariableDeclarationNode node )
        => Evaluator.Evaluate( this, node );

    private class MockVariableDeclarationEvaluator : IVariableDeclarationEvaluator
    {
        public IAstNode Evaluate( IAstVisitor visitor, AstVariableDeclarationNode node )
            => throw new System.NotImplementedException();
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
            throw new NotImplementedException();
            OutputUIInitializer( visitor, node, variable );
            return;
        }
        else if( variable.DataType.IsArray() )
        {
            throw new NotImplementedException();
            OutputArrayInitializer( visitor, node, variable );
            return;
        }

        throw new NotImplementedException();
        OutputPrimitiveInitializer( visitor, node, variable );
    }

    #region Primitive Initializer

    private void OutputPrimitiveInitializer( IAstVisitor visitor, AstVariableDeclarationNode node, VariableSymbol variable )
    {}

    #endregion ~Primitive Initializer


    #region Array Initializer

    private void OutputArrayInitializer( IAstVisitor visitor, AstVariableDeclarationNode node, VariableSymbol variable )
    {}

    private void OutputArraySize( IAstVisitor visitor, AstVariableDeclarationNode node, AstArrayInitializerNode initializer, VariableSymbol variable )
    {}

    private void OutputArrayElements( IAstVisitor visitor, AstVariableDeclarationNode node, VariableSymbol variable, AstArrayInitializerNode initializer )
    {}

    #endregion ~Array Initializer

    #region UI Initializer

    private void OutputUIInitializer( IAstVisitor visitor, AstVariableDeclarationNode node, VariableSymbol variable )
    {}

    private void OutputArrayBasedUIInitializer( IAstVisitor visitor, AstVariableDeclarationNode node, AstArrayInitializerNode initializer, VariableSymbol variable )
    {}

    private void OutputPrimitiveBasedUIInitializer( IAstVisitor visitor, AstVariableDeclarationNode node, AstExpressionListNode initializer, VariableSymbol variable )
    {}

    private void OutputUIArguments( IAstVisitor visitor, AstVariableDeclarationNode node, AstExpressionListNode expressionList, UITypeSymbol uiType )
    {}

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

public class ObfuscatedSymbolTable<TSymbol> : IObfuscatedTable<TSymbol> where TSymbol : SymbolBase
{
    // <original name, obfuscated name>
    private Dictionary<string, string> ObfuscatedTable { get; } = new();

    private string Prefix { get; }

    private IObfuscateFormatter Formatter { get; }

    public ObfuscatedSymbolTable( ISymbolTable<TSymbol> source, string prefix )
        : this( source, prefix, new DefaultObfuscateFormatter() ) {}

    public ObfuscatedSymbolTable( ISymbolTable<TSymbol> source, string prefix, IObfuscateFormatter formatter )
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
        => ObfuscatedTable[ original ];
}
