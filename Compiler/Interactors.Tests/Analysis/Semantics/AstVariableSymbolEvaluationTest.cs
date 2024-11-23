using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Domain.Symbols.MetaData.Extensions;
using KSPCompiler.Interactors.Analysis.Semantics;

using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

[TestFixture]
public class AstVariableSymbolEvaluationTest
{
    private static AstExpressionNode VariableSymbolTestBody( VariableSymbol variable )
    {
        var visitor = new MockAstSymbolVisitor();
        var compilerMessageManger = ICompilerMessageManger.Default;
        var symbolTable = MockUtility.CreateAggregateSymbolTable();

        symbolTable.Variables.Add( variable );

        var symbolEvaluator = new SymbolEvaluator( compilerMessageManger, symbolTable );
        visitor.Inject( symbolEvaluator );

        var node = MockUtility.CreateSymbolNode( variable.Name );
        var result = visitor.Visit( node );

        compilerMessageManger.WriteTo( Console.Out );

        return (AstExpressionNode)result;
    }

    [TestCase( false )]
    [TestCase( true )]
    public void IntVariableSymbolTest( bool isConstant )
    {
        var variable = MockUtility.CreateVariable( "$x", DataTypeFlag.TypeInt );

        if( isConstant )
        {
            variable.Modifier |= ModifierFlag.Const;
            variable.Value            =  1;
        }

        variable.State = SymbolState.Initialized;

        var result = VariableSymbolTestBody( variable );

        ClassicAssert.That(
            result,
            isConstant
                ? Is.InstanceOf<AstIntLiteralNode>()
                : Is.InstanceOf<AstSymbolExpressionNode>()
        );

        ClassicAssert.IsTrue( result.TypeFlag.IsInt() );
    }

    [TestCase( 123)]
    [TestCase( -123)]
    public void EvaluateConvolutionIntVariableTest( int value )
    {
        var variable = MockUtility.CreateVariable( "$x", DataTypeFlag.TypeInt );
        variable.Modifier |= ModifierFlag.Const;
        variable.Value    =  value;
        variable.State    = SymbolState.Initialized;

        var result = VariableSymbolTestBody( variable );
        var literal = result as AstIntLiteralNode;

        ClassicAssert.AreEqual( true, result.Constant );
        ClassicAssert.IsNotNull( literal );
        ClassicAssert.AreEqual( value, literal?.Value );
    }

    [TestCase( 1.23)]
    [TestCase( -1.23)]
    public void EvaluateConvolutionRealVariableTest( double value )
    {
        var variable = MockUtility.CreateRealVariable( "~x" );
        variable.Modifier |= ModifierFlag.Const;
        variable.Value    =  value;
        variable.State    =  SymbolState.Initialized;

        var result = VariableSymbolTestBody( variable );
        var literal = result as AstRealLiteralNode;

        ClassicAssert.AreEqual( true, result.Constant );
        ClassicAssert.IsNotNull( literal );
        ClassicAssert.AreEqual( value, literal?.Value );
    }

    [TestCase( "abc")]
    [TestCase( "ABC")]
    public void EvaluateConvolutionStringVariableTest( string value )
    {
        var variable = MockUtility.CreateStringVariable( "@x" );
        variable.Modifier |= ModifierFlag.Const;
        variable.Value    =  value;
        variable.State    =  SymbolState.Initialized;

        var result = VariableSymbolTestBody( variable );
        var literal = result as AstStringLiteralNode;

        ClassicAssert.AreEqual( true, result.Constant );
        ClassicAssert.IsNotNull( literal );
        ClassicAssert.AreEqual( value, literal?.Value );
    }

    [Test]
    public void CannotEvaluateNonDeclaredVariableTest()
    {
        var visitor = new MockAstSymbolVisitor();
        var compilerMessageManger = ICompilerMessageManger.Default;
        var symbolTable = MockUtility.CreateAggregateSymbolTable(); // no variables registered

        var symbolEvaluator = new SymbolEvaluator( compilerMessageManger, symbolTable );
        visitor.Inject( symbolEvaluator );

        var node = MockUtility.CreateSymbolNode( "$x" );
        visitor.Visit( node );

        compilerMessageManger.WriteTo( Console.Out );

        ClassicAssert.AreEqual( 1, compilerMessageManger.Count( CompilerMessageLevel.Error ) );
    }
}
