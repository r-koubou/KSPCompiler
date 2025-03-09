using System;

using KSPCompiler.Features.Compilation.Domain.Messages;
using KSPCompiler.Features.Compilation.Domain.Messages.Extensions;
using KSPCompiler.Features.Compilation.Gateways.EventEmitting;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics;
using KSPCompiler.Features.SymbolManagement.UseCase.Tests.Commons;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Blocks;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Statements;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;
using KSPCompiler.Shared.EventEmitting.Extensions;

using NUnit.Framework;

namespace KSPCompiler.Features.SymbolManagement.UseCase.Tests.Analysis.Semantics;

[TestFixture]
public class AstSelectStatementTest
{
    #region Common test methods

    private static void SelectStatementEvaluationTestBody( Action<AstSelectStatementNode> setup, int expectedErrorCount, int expectedWarningCount = 0 )
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );
        eventEmitter.Subscribe<CompilationWarningEvent>( e => compilerMessageManger.Warning( e.Position, e.Message ) );

        var visitor = new MockSelectStatementVisitor();
        var evaluator = new SelectStatementEvaluator( eventEmitter );

        visitor.Inject( evaluator );

        var statement = new AstSelectStatementNode();
        setup( statement );

        evaluator.Evaluate( visitor, statement );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Warning ), Is.EqualTo( expectedWarningCount ) );
        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ),   Is.EqualTo( expectedErrorCount ) );
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
