using System.Collections.Generic;
using System.Text;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Extensions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Interactors.Analysis.Obfuscators.Extensions;
using KSPCompiler.UseCases.Analysis.Evaluations.Declarations;
using KSPCompiler.UseCases.Analysis.Obfuscators;

namespace KSPCompiler.Interactors.Analysis.Obfuscators;

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

        if( variable.DataType.IsArray() )
        {
            OutputArrayInitializer( visitor, node );
            return;
        }

        OutputPrimitiveInitializer( visitor, node );
    }

    #region Primitive Initializer

    private void OutputPrimitiveInitializer( IAstVisitor visitor, AstVariableDeclarationNode node )
    {
        OutputBuilder.Append( " := " );
        node.Initializer.PrimitiveInitializer.Accept( visitor );
    }

    #endregion ~Primitive Initializer


    #region Array Initializer

    private void OutputArrayInitializer( IAstVisitor visitor, AstVariableDeclarationNode node )
    {
        OutputArraySize( visitor, node.Initializer.ArrayInitializer );
        OutputArrayElements( visitor, node.Initializer.ArrayInitializer );
    }

    private void OutputArraySize( IAstVisitor visitor, AstArrayInitializerNode initializer )
    {
        OutputBuilder.Append( '[' );
        initializer.Size.Accept( visitor );
        OutputBuilder.Append( ']' );
    }

    private void OutputArrayElements( IAstVisitor visitor, AstArrayInitializerNode initializer )
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
            OutputArrayBasedUIInitializer( visitor, node );
        }
        else
        {
            OutputPrimitiveBasedUIInitializer( visitor, node.Initializer.PrimitiveInitializer.UIInitializer );
        }
    }

    private void OutputArrayBasedUIInitializer( IAstVisitor visitor, AstVariableDeclarationNode node )
    {
        OutputArraySize( visitor, node.Initializer.ArrayInitializer );
        OutputUIArguments( visitor, node.Initializer.PrimitiveInitializer.UIInitializer );
    }

    private void OutputPrimitiveBasedUIInitializer( IAstVisitor visitor, AstExpressionListNode initializer )
    {
        OutputUIArguments( visitor, initializer );
    }

    private void OutputUIArguments( IAstVisitor visitor, AstExpressionListNode expressionList )
    {
        OutputBuilder.Append( " := " );
        OutputBuilder.Append( '(' );

        OutputBuilder.AppendExpressionList( visitor, expressionList );

        OutputBuilder.Append( ')' );
    }

    #endregion ~UI Initializer
}
