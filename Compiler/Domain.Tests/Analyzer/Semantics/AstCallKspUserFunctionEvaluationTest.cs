using System;

using KSPCompiler.Domain.Ast.Extensions;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Resources;

using NUnit.Framework;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

[TestFixture]
public class AstCallKspUserFunctionEvaluationTest
{
    [Test]
    public void CallFunctionTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var symbols = MockUtility.CreateAggregateSymbolTable();

        // register function `my_function`
        var function = MockUtility.CreateKspUserFunction( "my_function" );

        // register the function
        symbols.UserFunctions.Add( function );

        // Create a call function statement node
        // call my_function
        var callFunctionAst = MockUtility.CreateCallKspUserFunctionNode( function );

        var evaluator = new CallKspUserFunctionStatementEvaluator( compilerMessageManger, symbols.UserFunctions );
        var visitor = new MockCallKspUserFunctionStatementVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, callFunctionAst );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.AreEqual( 0, compilerMessageManger.Count( CompilerMessageLevel.Error ) );
    }

    [Test]
    public void CannotCallNotRegisteredFunctionTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var symbols = MockUtility.CreateAggregateSymbolTable();

        // Don't register the function for make a error
        // var function = MockUtility.CreateKspUserFunction( "my_function" );
        // symbols.UserFunctions.Add( function );

        // Creation of a non-existent call command expression node
        var callFunctionAst = MockUtility.CreateCallKspUserFunctionNode( "my_function" );

        var evaluator = new CallKspUserFunctionStatementEvaluator( compilerMessageManger, symbols.UserFunctions );
        var visitor = new MockCallKspUserFunctionStatementVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, callFunctionAst );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.AreEqual( 1, compilerMessageManger.Count( CompilerMessageLevel.Error ) );
    }
}

#region Work mock classes

public class MockCallKspUserFunctionEvaluator : ICallKspUserFunctionEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstCallKspUserFunctionStatementNode statement )
    {
        throw new NotImplementedException();
    }
}

public class MockCallKspUserFunctionStatementVisitor : DefaultAstVisitor
{
    private ICallKspUserFunctionEvaluator Evaluator { get; set; } = new MockCallKspUserFunctionEvaluator();

    public void Inject( ICallKspUserFunctionEvaluator evaluator )
    {
        Evaluator = evaluator;
    }

    public override IAstNode Visit( AstCallKspUserFunctionStatementNode node )
        => Evaluator.Evaluate( this, node );
}

#endregion

public interface ICallKspUserFunctionEvaluator
{
    IAstNode Evaluate( IAstVisitor visitor, AstCallKspUserFunctionStatementNode statement );
}

public class CallKspUserFunctionStatementEvaluator : ICallKspUserFunctionEvaluator
{
    private ICompilerMessageManger CompilerMessageManger { get; }
    private IUserFunctionSymbolSymbolTable UserFunctions { get; }

    public CallKspUserFunctionStatementEvaluator( ICompilerMessageManger compilerMessageManger, IUserFunctionSymbolSymbolTable symbolTable )
    {
        CompilerMessageManger = compilerMessageManger;
        UserFunctions         = symbolTable;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstCallKspUserFunctionStatementNode statement )
    {
        if( !UserFunctions.TrySearchByName( statement.Name, out _ ) )
        {
            CompilerMessageManger.Error(
                statement,
                CompilerMessageResources.semantic_error_userfunction_ksp_unknown,
                statement.Name
            );
        }

        return statement.Clone<AstCallKspUserFunctionStatementNode>();
    }
}
