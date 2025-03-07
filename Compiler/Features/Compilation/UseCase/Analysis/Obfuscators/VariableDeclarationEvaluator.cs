using System.Collections.Generic;
using System.Text;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Declarations;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Obfuscators;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Extensions;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators.Extensions;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Extensions;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Statements;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData.Extensions;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators;

public class VariableDeclarationEvaluator(
    StringBuilder output,
    AggregateSymbolTable symbolTable,
    AggregateObfuscatedSymbolTable obfuscatedTable )
    : IVariableDeclarationEvaluator
{
    private StringBuilder Output { get; } = output;
    private AggregateSymbolTable SymbolTable { get; } = symbolTable;
    private IObfuscatedVariableTable ObfuscatedTable { get; } = obfuscatedTable.Variables;

    public IAstNode Evaluate( IAstVisitor visitor, AstVariableDeclarationNode node )
    {
        var name = ObfuscatedTable.GetObfuscatedByName( node.Name );

        if( !SymbolTable.TrySearchUserVariableByName( node.Name, out var variable ) )
        {
            throw new KeyNotFoundException( $"Variable not found: {node.Name} from variable symbol table" );
        }

        // 定数変数はシュリンク
        // 意味解析フェーズで定数変数の初期化式を評価済み＆畳み込み済みのため
        if( variable.Modifier.IsConstant() )
        {
            return node;
        }

        // 未使用の変数はシュリンク
        // （ただし、UI変数は宣言・初期化時点で有効・画面に配置される物もあるため、シュリンク対象外）
        if( variable.UIType == UITypeSymbol.Null && variable.State.IsNotUsed() )
        {
            return node;
        }

        Output.Append( "declare" );

        foreach( var modifier in node.Modifier.Values )
        {
            Output.Append( $" {modifier}" );
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

        if( node.Initializer.ArrayInitializer.HasAssignOperator )
        {
            Output.Append( " := " );
        }

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
        if( initializer.Initializer.Empty )
        {
            return;
        }

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

        if( node.Initializer.ArrayInitializer.HasAssignOperator )
        {
            Output.Append( " := " );
        }

        OutputUIArguments( visitor, node.Initializer.ArrayInitializer.Initializer );
    }

    private void OutputPrimitiveBasedUIInitializer( IAstVisitor visitor, AstExpressionListNode initializer )
    {
        OutputUIArguments( visitor, initializer );
    }

    private void OutputUIArguments( IAstVisitor visitor, AstExpressionListNode expressionList )
    {
        if( expressionList.Empty )
        {
            return;
        }

        Output.Append( '(' );

        Output.AppendExpressionList( visitor, expressionList );

        Output.Append( ')' );
    }

    #endregion ~UI Initializer
}
