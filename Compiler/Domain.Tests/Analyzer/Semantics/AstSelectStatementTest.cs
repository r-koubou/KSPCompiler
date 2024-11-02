using System;

using KSPCompiler.Domain.Ast.Analyzers;
using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Conditionals;
using KSPCompiler.Domain.Ast.Extensions;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Extensions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Resources;

using NUnit.Framework;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

[TestFixture]
public class AstSelectStatementTest
{
    #region Common test methods

    private static void SelectStatementEvaluationTestBody( Action<AstSelectStatementNode> setup, int expectedErrorCount, int expectedWarningCount = 0 )
    {
        var compilerMessageManger = ICompilerMessageManger.Default;

        var visitor = new MockSelectStatementVisitor();
        var evaluator = new SelectStatementEvaluator( compilerMessageManger );

        visitor.Inject( evaluator );

        var statement = new AstSelectStatementNode();
        setup( statement );

        evaluator.Evaluate( visitor, statement );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.AreEqual( expectedWarningCount, compilerMessageManger.Count( CompilerMessageLevel.Warning ) );
        Assert.AreEqual( expectedErrorCount,   compilerMessageManger.Count( CompilerMessageLevel.Error ) );
    }

    private static void AddCaseBlock( AstSelectStatementNode statement, AstExpressionNode caseFrom, AstStatementNode caseCode )
    {
        var caseNode = new AstCaseBlock( statement.Condition )
        {
            ConditionFrom = caseFrom
        };

        caseNode.CodeBlock.Statements.Add( caseCode );
        statement.CaseBlocks.Add( caseNode );
    }

    private static void AddCaseFromToBlock( AstSelectStatementNode statement, AstExpressionNode caseFrom, AstExpressionNode caseTo, AstStatementNode caseCode )
    {
        var caseNode = new AstCaseBlock( statement.Condition )
        {
            ConditionFrom = caseFrom,
            ConditionTo = caseTo
        };

        caseNode.CodeBlock.Statements.Add( caseCode );
        statement.CaseBlocks.Add( caseNode );
    }

    #endregion

    [Test]
    public void SelectEvaluationTest()
    {
        // select( $x )
        //    case 1
        //       { do something }
        //    case 2
        //       { do something }
        // end select
        SelectStatementEvaluationTestBody(
            statement =>
            {
                statement.Condition = new AstSymbolExpressionNode
                {
                    Parent   = statement,
                    Name     = "$x",
                    TypeFlag = DataTypeFlag.TypeInt
                };

                AddCaseBlock( statement, new AstIntLiteralNode( 1 ), new MockAstStatementNode( statement ) );
                AddCaseBlock( statement, new AstIntLiteralNode( 2 ), new MockAstStatementNode( statement ) );
            },
            0
        );
    }

    [Test]
    public void SelectFromToEvaluationTest()
    {
        // select( $x )
        //    case 1 to 2
        //       { do something }
        // end select
        SelectStatementEvaluationTestBody(
            statement =>
            {
                statement.Condition = new AstSymbolExpressionNode
                {
                    Parent   = statement,
                    Name     = "$x",
                    TypeFlag = DataTypeFlag.TypeInt
                };

                AddCaseFromToBlock(
                    statement: statement,
                    caseFrom: new AstIntLiteralNode( 1 ),
                    caseTo: new AstIntLiteralNode( 2 ),
                    caseCode: new MockAstStatementNode( statement )
                );
            },
            0
        );
    }

    [Test]
    public void CannotEvaluateCaseIsNotConstantTest()
    {
        // $x is not declared as constant
        var variableX = new AstSymbolExpressionNode
        {
            Name     = "$x",
            TypeFlag = DataTypeFlag.TypeInt
        };

        // select( $x )
        //    case $x <-- case value must be constant
        //       { do something }
        // end select
        SelectStatementEvaluationTestBody(
            statement =>
            {
                statement.Condition = variableX;
                AddCaseBlock( statement, variableX, new MockAstStatementNode( statement ) );
            },
            1
        );
    }

    [Test]
    public void CannotEvaluateCaseIsIncompatibleTest()
    {
        // $x is not declared as constant
        var variableX = new AstSymbolExpressionNode
        {
            Name     = "$x",
            TypeFlag = DataTypeFlag.TypeInt
        };

        // select( $x )
        //    case "a" <-- case value must be integer constant
        //       { do something }
        // end select
        SelectStatementEvaluationTestBody(
            statement =>
            {
                statement.Condition = variableX;
                AddCaseBlock( statement, new AstStringLiteralNode( "a" ), new MockAstStatementNode( statement ) );
            },
            1
        );
    }

    [Test]
    public void CannotCaseFromGreaterThanToTest()
    {
        // select( $x )
        //    case 2 to 1 <-- error: from is greater than to
        //       { do something }
        // end select
        SelectStatementEvaluationTestBody(
            statement =>
            {
                statement.Condition = new AstSymbolExpressionNode
                {
                    Parent   = statement,
                    Name     = "$x",
                    TypeFlag = DataTypeFlag.TypeInt
                };

                AddCaseFromToBlock(
                    statement: statement,
                    caseFrom: new AstIntLiteralNode( 2 ),
                    caseTo: new AstIntLiteralNode( 1 ),
                    caseCode: new MockAstStatementNode( statement )
                );
            },
            1
        );
    }

    [Test]
    public void WarningIfFromAndToIsEqualTest()
    {
        // select( $x )
        //    case 1 to 1 <-- warning: from and to are equal
        //       { do something }
        // end select
        SelectStatementEvaluationTestBody(
            statement =>
            {
                statement.Condition = new AstSymbolExpressionNode
                {
                    Parent   = statement,
                    Name     = "$x",
                    TypeFlag = DataTypeFlag.TypeInt
                };

                AddCaseFromToBlock(
                    statement: statement,
                    caseFrom: new AstIntLiteralNode( 1 ),
                    caseTo: new AstIntLiteralNode( 1 ),
                    caseCode: new MockAstStatementNode( statement )
                );
            },
            0,
            1
        );
    }
}

#region Work mock classes

public class MockAstStatementNode : AstStatementNode
{
    public MockAstStatementNode(IAstNode parent ) : base( AstNodeId.None, parent ) {}

    public override int ChildNodeCount
        => 0;

    public override IAstNode Accept( IAstVisitor visitor )
        => NullAstNode.Instance;

    public override void AcceptChildren( IAstVisitor visitor ) {}
}

public class MockSelectStatementEvaluator : ISelectStatementEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstSelectStatementNode statement )
        => throw new NotImplementedException();
}

public class MockSelectStatementVisitor : DefaultAstVisitor
{
    private ISelectStatementEvaluator Evaluator { get; set; } = new MockSelectStatementEvaluator();

    public void Inject( ISelectStatementEvaluator evaluator )
    {
        Evaluator = evaluator;
    }

    public override IAstNode Visit( AstSelectStatementNode node )
        => Evaluator.Evaluate( this, node );
}

#endregion

public interface ISelectStatementEvaluator : IConditionalStatementEvaluator<AstSelectStatementNode> {}

public class SelectStatementEvaluator : ISelectStatementEvaluator
{
    private ICompilerMessageManger CompilerMessageManger { get; }

    public SelectStatementEvaluator( ICompilerMessageManger compilerMessageManger )
    {
        CompilerMessageManger = compilerMessageManger;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstSelectStatementNode statement )
    {
        if( statement.Condition.Accept( visitor ) is not AstExpressionNode evaluatedCondition )
        {
            throw new AstAnalyzeException( statement, "Failed to evaluate condition" );
        }

        // selectの評価対象が変数ではない場合
        if( evaluatedCondition is not AstSymbolExpressionNode )
        {
            CompilerMessageManger.Error(
                statement,
                CompilerMessageResources.semantic_error_select_condition_notvariable
            );

            return statement.Clone<AstSelectStatementNode>();
        }

        // selectの評価対象が整数ではない場合
        if( evaluatedCondition.TypeFlag != DataTypeFlag.TypeInt )
        {
            CompilerMessageManger.Error(
                statement,
                CompilerMessageResources.semantic_error_select_condition_incompatible
            );

            return statement.Clone<AstSelectStatementNode>();
        }

        foreach( var caseBlock in statement.CaseBlocks )
        {
            EvaluateCaseBlock( visitor, statement, caseBlock );
        }

        return statement.Clone<AstSelectStatementNode>();
    }

    private void EvaluateCaseBlock( IAstVisitor visitor, AstSelectStatementNode statement, AstCaseBlock caseBlock )
    {
        if( caseBlock.ConditionFrom.Accept( visitor ) is not AstExpressionNode evaluatedCaseFrom )
        {
            throw new AstAnalyzeException( statement, "Failed to evaluate case condition" );
        }

        // case 条件が整数かつ定数ではない場合
        if( evaluatedCaseFrom.TypeFlag != DataTypeFlag.TypeInt || !evaluatedCaseFrom.Constant )
        {
            CompilerMessageManger.Error(
                statement,
                CompilerMessageResources.semantic_error_select_case_incompatible
            );
        }

        // from to の場合
        if( caseBlock.ConditionTo.IsNotNull() )
        {
            if( caseBlock.ConditionTo.Accept( visitor ) is not AstExpressionNode evaluatedCaseTo )
            {
                throw new AstAnalyzeException( statement, "Failed to evaluate case condition" );
            }

            // case 条件が整数かつ定数ではない場合
            if( evaluatedCaseTo.TypeFlag != DataTypeFlag.TypeInt || !evaluatedCaseTo.Constant )
            {
                CompilerMessageManger.Error(
                    statement,
                    CompilerMessageResources.semantic_error_select_case_incompatible
                );

                return;
            }

            EvaluateCaseRange( statement, evaluatedCaseFrom, evaluatedCaseTo );
        }

        // case 内のコードブロックの評価
        caseBlock.CodeBlock.AcceptChildren( visitor );
    }

    // from to の値が確定している場合の範囲が正しいかチェック
    private void EvaluateCaseRange( AstSelectStatementNode statement, AstExpressionNode caseFrom, AstExpressionNode caseTo )
    {
        if( caseFrom is not AstIntLiteralNode caseFromLiteral )
        {
            return;
        }

        if( caseTo is not AstIntLiteralNode caseToLiteral )
        {
            return;
        }

        // case <from> と case <to> が同じ値の場合
        if( caseFromLiteral.Value == caseToLiteral.Value )
        {
            CompilerMessageManger.Warning(
                statement,
                CompilerMessageResources.semantic_warning_select_case_from_to_noeffect,
                caseFromLiteral.Value
            );

            return;
        }

        // case <from> が case <to> より大きい場合
        if( caseFromLiteral.Value > caseToLiteral.Value )
        {
            CompilerMessageManger.Error(
                statement,
                CompilerMessageResources.semantic_error_select_case_from_grater,
                caseFromLiteral.Value
            );
        }
    }
}
