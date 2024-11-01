using System;

using KSPCompiler.Domain.Ast.Analyzers;
using KSPCompiler.Domain.Ast.Extensions;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Resources;

using NUnit.Framework;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

[TestFixture]
public class AstIfStatementTest
{
    #region Common test methods

    private static void IfStatementEvaluationTestBody(
        AstExpressionNode condition,
        AstBlockNode codeBlock,
        AstBlockNode elseBlock,
        int expectedErrorCount )
    {
        var compilerMessageManger = ICompilerMessageManger.Default;

        var visitor = new MockIfStatementVisitor();
        var evaluator = new IfStatementEvaluator( compilerMessageManger );

        visitor.Inject( evaluator );

        var ast = new AstIfStatementNode
        {
            Condition = condition,
            CodeBlock = codeBlock,
            ElseBlock = elseBlock
        };

        ast.Condition.Parent = ast;
        ast.CodeBlock.Parent = ast;
        ast.ElseBlock.Parent = ast;

        evaluator.Evaluate( visitor, ast );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.AreEqual( expectedErrorCount, compilerMessageManger.Count( CompilerMessageLevel.Error ) );
    }

    private static void IfStatementEvaluationTestBody( AstExpressionNode condition, AstBlockNode codeBlock, int expectedErrorCount )
        => IfStatementEvaluationTestBody( condition, codeBlock, new AstBlockNode(), expectedErrorCount );

    private static void IfStatementEvaluationTestBody( AstExpressionNode condition, int expectedErrorCount )
        => IfStatementEvaluationTestBody( condition, new AstBlockNode(), new AstBlockNode(), expectedErrorCount );

    #endregion

    [Test]
    public void ConditionEvaluationTest()
    {
        // if( 1 = 1 )
        // end if
        var condition = new AstEqualExpressionNode
        {
            // set an evaluated type value here because statement's unit test only here
            TypeFlag = DataTypeFlag.TypeBool,
            Left   = new AstIntLiteralNode( 1 ),
            Right  = new AstIntLiteralNode( 1 )
        };

        IfStatementEvaluationTestBody( condition, 0 );
    }

    [Test]
    public void ElseIfEvaluationTest()
    {
        // if( 1 = 2 )
        var condition = new AstEqualExpressionNode
        {
            // set an evaluated type value here because statement's unit test only here
            TypeFlag = DataTypeFlag.TypeBool,
            Left     = new AstIntLiteralNode( 1 ),
            Right    = new AstIntLiteralNode( 2 )
        };

        // else if( 2 = 2 )
        var elseBlock = new AstBlockNode();
        elseBlock.Statements.Add( new AstIfStatementNode
            {
                Condition = new AstEqualExpressionNode
                {
                    // set an evaluated type value here because statement's unit test only here
                    TypeFlag = DataTypeFlag.TypeBool,
                    Left     = new AstIntLiteralNode( 2 ),
                    Right    = new AstIntLiteralNode( 2 )
                },
                CodeBlock = new AstBlockNode()
            }
        );

        IfStatementEvaluationTestBody( condition, new AstBlockNode(), elseBlock, 0 );
    }

    [Test]
    public void CannotEvaluateWithoutConditionTest()
    {
        // if() <-- no condition
        // end if
        var condition = NullAstExpressionNode.Instance;

        IfStatementEvaluationTestBody( condition, 1 );
    }

    [Test]
    public void CannotEvaluateWithIncompatibleConditionTypeTest()
    {
        // if( 1 + 1 ) <-- incompatible condition type (its binary expression)
        // end if
        var condition = new AstAdditionExpressionNode
        {
            Left   = new AstIntLiteralNode( 1 ),
            Right  = new AstIntLiteralNode( 1 )
        };

        IfStatementEvaluationTestBody( condition, 1 );
    }

}

#region Work mock classes

public class MockIfStatementEvaluator : IIfStatementEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstIfStatementNode statement )
        => throw new NotImplementedException();
}

public class MockIfStatementVisitor : DefaultAstVisitor
{
    private IIfStatementEvaluator IfStatementEvaluator { get; set; } = new MockIfStatementEvaluator();

    public void Inject( IIfStatementEvaluator evaluator )
        => IfStatementEvaluator = evaluator;

    public override IAstNode Visit( AstIfStatementNode node )
        => IfStatementEvaluator.Evaluate( this, node );
}

#endregion

public interface IConditionalStatementEvaluator<in TStatementNode> where TStatementNode : AstStatementNode
{
    IAstNode Evaluate( IAstVisitor visitor, TStatementNode statement );
}

public interface IIfStatementEvaluator : IConditionalStatementEvaluator<AstIfStatementNode> {}

public class IfStatementEvaluator : IIfStatementEvaluator
{
    private ICompilerMessageManger CompilerMessageManger { get; }

    public IfStatementEvaluator( ICompilerMessageManger compilerMessageManger )
    {
        CompilerMessageManger = compilerMessageManger;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstIfStatementNode statement )
    {
        // 条件式の評価
        if( statement.Condition.Accept( visitor ) is not AstExpressionNode evaluatedCondition )
        {
            throw new AstAnalyzeException( statement, "Failed to evaluate condition" );
        }

        // if条件式が条件式以外の場合
        if( !evaluatedCondition.TypeFlag.IsBoolean() )
        {
            CompilerMessageManger.Error(
                statement,
                CompilerMessageResources.semantic_error_if_condition_incompatible
            );

            return statement.Clone<AstIfStatementNode>();
        }

        // 真の場合のコードブロックの評価
        statement.CodeBlock.AcceptChildren( visitor );

        // else節がある場合のコードブロックの評価
        statement.ElseBlock.AcceptChildren( visitor );

        // Memo
        // 畳み込みで条件式をリテラルでノードを置き換え可能だが
        // 文法上は条件式リテラルが許されていないため、ここでは畳み込みは行わない
        // 最適化やオブファスケーターの団で畳み込みを行う

        return statement.Clone<AstIfStatementNode>();
    }
}
