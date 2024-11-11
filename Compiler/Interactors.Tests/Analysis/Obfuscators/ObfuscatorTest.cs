using System.Text;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Extensions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.UseCases.Analysis;

using NUnit.Framework;

namespace KSPCompiler.Interactors.Tests.Analysis.Obfuscators;

[TestFixture]
public class ObfuscatorTest
{
    [Test]
    public void TestMethod()
    {
        var symbols = MockUtility.CreateAggregateSymbolTable();
        var stringBuilder = new StringBuilder();
        var obfuscator = new Obfuscator( stringBuilder, symbols );

        Assert.Pass();
    }
}

public static class StringBuilderExtensions
{
    public static void NewLine( this StringBuilder stringBuilder )
    {
        stringBuilder.Append( '\n' );
    }
}

public class Obfuscator : DefaultAstVisitor, IAstTraversal
{
    private StringBuilder OutputBuilder { get; }
    private AggregateSymbolTable SymbolTable { get; }

    public Obfuscator( StringBuilder outputBuilder, AggregateSymbolTable symbolTable )
    {
        OutputBuilder = outputBuilder;
        SymbolTable   = symbolTable;
    }

    public void Traverse( AstCompilationUnitNode node )
    {
        node.AcceptChildren( this );
    }

    private void WriteBinaryOperator( string op, IAstNode left, IAstNode right )
    {
        left.Accept( this );
        OutputBuilder.Append( $" {op} " );
        right.Accept( this );
    }

    private void WriteUnaryOperator( string op, IAstNode left )
    {
        OutputBuilder.Append( op );
        left.Accept( this );
    }

    #region Declarations

    public override IAstNode Visit( AstCallbackDeclarationNode node )
    {
        return node;
    }

    public override IAstNode Visit( AstUserFunctionDeclarationNode node )
    {
        return node;
    }

    public override IAstNode Visit( AstVariableDeclarationNode node )
    {
        return node;
    }

    #endregion ~Declarations

    #region Expressions

    #region Assignments

    public override IAstNode Visit( AstAssignmentExpressionNode node )
    {
        node.Left.Accept( this );
        OutputBuilder.Append( ":=" );
        node.Right.Accept( this );
        OutputBuilder.NewLine();
        return node;
    }

    #endregion ~Assignments

    #region Operators

    #region Binary Operators

    public override IAstNode Visit( AstAdditionExpressionNode node )
    {
        WriteBinaryOperator( "+", node.Left, node.Right );
        return node;
    }

    public override IAstNode Visit( AstSubtractionExpressionNode node )
    {
        WriteBinaryOperator( "-", node.Left, node.Right );
        return node;
    }

    public override IAstNode Visit( AstMultiplyingExpressionNode node )
    {
        WriteBinaryOperator( "*", node.Left, node.Right );
        return node;
    }

    public override IAstNode Visit( AstDivisionExpressionNode node )
    {
        WriteBinaryOperator( "/", node.Left, node.Right );
        return node;
    }

    public override IAstNode Visit( AstModuloExpressionNode node )
    {
        WriteBinaryOperator( "mod", node.Left, node.Right );
        return node;
    }

    public override IAstNode Visit( AstBitwiseOrExpressionNode node )
    {
        WriteBinaryOperator( ".or.", node.Left, node.Right );
        return node;
    }

    public override IAstNode Visit( AstBitwiseAndExpressionNode node )
    {
        WriteBinaryOperator( ".and.", node.Left, node.Right );
        return node;
    }

    public override IAstNode Visit( AstBitwiseXorExpressionNode node )
    {
        WriteBinaryOperator( ".xor.", node.Left, node.Right );
        return node;
    }

    #endregion ~Binary Operators

    #region String concatenation operator

    public override IAstNode Visit( AstStringConcatenateExpressionNode node )
    {
        WriteBinaryOperator( "&", node.Left, node.Right );
        return node;
    }

    #region Unary Operators

    public override IAstNode Visit( AstUnaryNotExpressionNode node )
    {
        return node;
    }

    public override IAstNode Visit( AstUnaryMinusExpressionNode node )
    {
        return node;
    }

    #endregion ~Unary Operators

    #endregion

    #region Conditional Binary Operators
    public override IAstNode Visit( AstEqualExpressionNode node )
    {
        WriteBinaryOperator( "=", node.Left, node.Right );
        return node;
    }

    public override IAstNode Visit( AstNotEqualExpressionNode node )
    {
        WriteBinaryOperator( "#", node.Left, node.Right );
        return node;
    }

    public override IAstNode Visit( AstLessThanExpressionNode node )
    {
        WriteBinaryOperator( "<", node.Left, node.Right );
        return node;
    }

    public override IAstNode Visit( AstGreaterThanExpressionNode node )
    {
        WriteBinaryOperator( ">", node.Left, node.Right );
        return node;
    }

    public override IAstNode Visit( AstLessEqualExpressionNode node )
    {
        WriteBinaryOperator( "<=", node.Left, node.Right );
        return node;
    }

    public override IAstNode Visit( AstGreaterEqualExpressionNode node )
    {
        WriteBinaryOperator( ">=", node.Left, node.Right );
        return node;
    }

    #endregion ~Conditional Binary Operators

    #region Conditional Logical Operators

    public override IAstNode Visit( AstLogicalOrExpressionNode node )
    {
        WriteBinaryOperator( "or", node.Left, node.Right );
        return node;
    }

    public override IAstNode Visit( AstLogicalAndExpressionNode node )
    {
        WriteBinaryOperator( "and", node.Left, node.Right );
        return node;
    }

    public override IAstNode Visit( AstLogicalXorExpressionNode node )
    {
        WriteBinaryOperator( "xor", node.Left, node.Right );
        return node;
    }

    public override IAstNode Visit( AstUnaryLogicalNotExpressionNode node )
    {
        WriteUnaryOperator( "not", node.Left );
        return node;
    }

    #endregion ~Conditional Logical Operators

    #region Symbols

    public override IAstNode Visit( AstSymbolExpressionNode node )
    {
        return node;
    }

    public override IAstNode Visit( AstArrayElementExpressionNode node )
    {
        OutputBuilder.Append( '[' );
        node.Left.Accept( this );
        OutputBuilder.Append( ']' );
        return node;
    }

    public override IAstNode Visit( AstCallCommandExpressionNode node )
    {
        var command = (AstExpressionNode)node.Left.Accept( this );
        var arguments = (AstExpressionListNode)node.Right;

        OutputBuilder.Append( command.Name );
        OutputBuilder.Append( '(' );

        var i = 0;
        foreach( var arg in arguments.Expressions )
        {
            arg.Accept( this );

            if( i < arguments.Expressions.Count - 1 )
            {
                OutputBuilder.Append( ", " );
            }

            i++;
        }

        OutputBuilder.Append( ')' ).NewLine();

        return node;
    }

    #endregion ~|Symbols

    #endregion ~Operators

    #endregion ~Expressions

    #region Statements

    #region Preprocessor Symbol Statements

    public override IAstNode Visit( AstPreprocessorIfdefineNode node )
    {
        if( node.Ignore )
        {
            return node;
        }

        node.Block.AcceptChildren( this );

        return node;
    }

    public override IAstNode Visit( AstPreprocessorIfnotDefineNode node )
    {
        if( node.Ignore )
        {
            return node;
        }

        node.Block.AcceptChildren( this );

        return node;
    }

    #endregion ~Preprocessor Symbol Statements

    #region Call User Function Statements

    public override IAstNode Visit( AstCallUserFunctionStatementNode node )
    {
        OutputBuilder.Append( "call " )
                     .Append( node.Name )
                     .NewLine();

        return node;
    }

    #endregion ~Call User Function Statements

    #region Control Statements

    public override IAstNode Visit( AstIfStatementNode node )
    {
        OutputBuilder.Append( "if" );
        OutputBuilder.Append( '(' );
        node.Condition.Accept( this );
        OutputBuilder.Append( ')' ).NewLine();

        node.CodeBlock.AcceptChildren( this );

        if( node.ElseBlock.IsNotNull() )
        {
            OutputBuilder.Append( "else" ).NewLine();
            node.ElseBlock.AcceptChildren( this );
        }

        OutputBuilder.Append( "end if" );
        OutputBuilder.NewLine();

        return node;
    }

    public override IAstNode Visit( AstWhileStatementNode node )
    {
        OutputBuilder.Append( "while" );
        OutputBuilder.Append( '(' );
        node.Condition.Accept( this );
        OutputBuilder.Append( ')' ).NewLine();

        node.CodeBlock.AcceptChildren( this );

        OutputBuilder.Append( "end while" );
        OutputBuilder.NewLine();
        return node;
    }



    public override IAstNode Visit( AstSelectStatementNode node )
    {
        OutputBuilder.Append( "select(" );
        node.Condition.Accept( this );
        OutputBuilder.Append( ')' ).NewLine();

        foreach( var caseBlock in node.CaseBlocks )
        {
            OutputBuilder.Append( "case " );
            caseBlock.ConditionFrom.Accept( this );

            if( caseBlock.ConditionTo.IsNotNull() )
            {
                OutputBuilder.Append( " to " );
                caseBlock.ConditionTo.Accept( this );
            }

            caseBlock.CodeBlock.AcceptChildren( this );
        }

        OutputBuilder.Append( "end select" );
        OutputBuilder.NewLine();

        return node;
    }

    public override IAstNode Visit( AstContinueStatementNode node )
    {
        OutputBuilder.Append( "continue" );
        OutputBuilder.NewLine();
        return node;
    }

    #endregion ~Control Statements

    #endregion ~Statements
}
