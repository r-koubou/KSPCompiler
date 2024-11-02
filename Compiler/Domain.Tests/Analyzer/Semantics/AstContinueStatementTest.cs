using System;

using KSPCompiler.Domain.Ast.Analyzers.Evaluators.Conditionals;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols.MetaData;

using NUnit.Framework;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

[TestFixture]
public class AstContinueStatementTest
{
    #region Common test methods

    private static void ContinueStatementEvaluationTestBody( AstContinueStatementNode node, int expectedErrorCount )
    {
        var compilerMessageManger = ICompilerMessageManger.Default;

        var visitor = new MockContinueStatementVisitor();
        var evaluator = new ContinueStatementEvaluator( compilerMessageManger );

        visitor.Inject( evaluator );

        evaluator.Evaluate( visitor, node );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.AreEqual( expectedErrorCount, compilerMessageManger.Count( CompilerMessageLevel.Error ) );
    }

    #endregion

    [Test]
    public void ConditionEvaluationTest()
    {
        // while( 1 # 0 )
        //    continue
        // end while
        var statement = new AstWhileStatementNode
        {
            Condition = new AstNotEqualExpressionNode
            {
                // set an evaluated type value here because statement's unit test only here
                TypeFlag = DataTypeFlag.TypeBool,
                Left     = new AstIntLiteralNode( 1 ),
                Right    = new AstIntLiteralNode( 0 )
            }
        };

        var continueStatement = new AstContinueStatementNode();
        statement.CodeBlock.Statements.Add( continueStatement );

        ContinueStatementEvaluationTestBody( continueStatement, 0 );
    }

    [Test]
    public void CannotEvaluateWithoutConditionTest()
    {
        // if( 1 = 1 )
        //    continue <-- continue statement available only in while loop
        // end if
        var statement = new AstIfStatementNode
        {
            Condition = new AstEqualExpressionNode
            {
                // set an evaluated type value here because statement's unit test only here
                TypeFlag = DataTypeFlag.TypeBool,
                Left     = new AstIntLiteralNode( 1 ),
                Right    = new AstIntLiteralNode( 1 )
            }
        };

        var continueStatement = new AstContinueStatementNode();
        statement.CodeBlock.Statements.Add( continueStatement );

        ContinueStatementEvaluationTestBody( continueStatement, 1 );
    }
}

#region Work mock classes

public class MockContinueStatementEvaluator : IContinueStatementEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstContinueStatementNode statement )
        => throw new NotImplementedException();
}

public class MockContinueStatementVisitor : DefaultAstVisitor
{
    private IContinueStatementEvaluator Evaluator { get; set; } = new MockContinueStatementEvaluator();

    public void Inject( IContinueStatementEvaluator evaluator )
    {
        Evaluator = evaluator;
    }

    public override IAstNode Visit( AstContinueStatementNode node )
        => Evaluator.Evaluate( this, node );
}

#endregion

public interface IContinueStatementEvaluator : IConditionalStatementEvaluator<AstContinueStatementNode> {}

public class ContinueStatementEvaluator : IContinueStatementEvaluator
{
    private ICompilerMessageManger CompilerMessageManger { get; }

    public ContinueStatementEvaluator( ICompilerMessageManger compilerMessageManger )
    {
        CompilerMessageManger = compilerMessageManger;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstContinueStatementNode statement )
    {
        throw new NotImplementedException();
    }
}
