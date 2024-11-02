using System;

using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Conditionals;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols.MetaData;

using NUnit.Framework;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

[TestFixture]
public class AstSelectStatementTest
{
    #region Common test methods

    private static void SelectStatementEvaluationTestBody( Action<AstSelectStatementNode> setup, int expectedErrorCount )
    {
        var compilerMessageManger = ICompilerMessageManger.Default;

        var visitor = new MockSelectStatementVisitor();
        var evaluator = new SelectStatementEvaluator( compilerMessageManger );

        visitor.Inject( evaluator );

        var statement = new AstSelectStatementNode();
        setup( statement );

        evaluator.Evaluate( visitor, statement );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.AreEqual( expectedErrorCount, compilerMessageManger.Count( CompilerMessageLevel.Error ) );

    }

    private static void AddCaseBlock( AstSelectStatementNode statement, AstExpressionNode caseFrom, AstStatementNode caseCode )
    {
        var caseNode = new AstCaseBlock( statement.Condition )
        {
            ConditionFrom = caseFrom,
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
        throw new NotImplementedException();
    }
}
