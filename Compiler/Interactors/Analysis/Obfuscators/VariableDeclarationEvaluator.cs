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
    private StringBuilder Output { get; }
    private IVariableSymbolTable Variables { get; }
    private ISymbolTable<UITypeSymbol> UITypeSymbols { get; }
    private IObfuscatedVariableTable ObfuscatedTable { get; }

    public VariableDeclarationEvaluator(
        StringBuilder output,
        IVariableSymbolTable variables,
        ISymbolTable<UITypeSymbol> uiTypeSymbols,
        IObfuscatedVariableTable obfuscatedTable )
    {
        Output          = output;
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

        // 未使用の変数はシュリンク
        if( variable.State.IsNotUsed() )
        {
            return node;
        }

        Output.Append( "declare" );

        foreach( var modifier in node.Modifier.Values )
        {
            Output.Append( $" {modifier}" );
        }

        if( variable.UIType != UITypeSymbol.Null )
        {
            Output.Append( $" {variable.UIType.Name}" );
        }

        Output.Append( $" {name}" );

        if( node.Initializer.IsNotNull() )
        {
            OutputInitializer( visitor, node, variable );
        }

        Output.NewLine();

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
        Output.Append( " := " );
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
        Output.Append( '[' );
        initializer.Size.Accept( visitor );
        Output.Append( ']' );
    }

    private void OutputArrayElements( IAstVisitor visitor, AstArrayInitializerNode initializer )
    {
        Output.Append( " := " );
        Output.Append( '(' );

        Output.AppendExpressionList( visitor, initializer.Initializer );

        Output.Append( ')' );
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
        OutputUIArguments( visitor, node.Initializer.ArrayInitializer.Initializer );
    }

    private void OutputPrimitiveBasedUIInitializer( IAstVisitor visitor, AstExpressionListNode initializer )
    {
        OutputUIArguments( visitor, initializer );
    }

    private void OutputUIArguments( IAstVisitor visitor, AstExpressionListNode expressionList )
    {
        Output.Append( " := " );
        Output.Append( '(' );

        Output.AppendExpressionList( visitor, expressionList );

        Output.Append( ')' );
    }

    #endregion ~UI Initializer
}
