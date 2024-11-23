using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Interactors.Analysis.Semantics;

using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

[TestFixture]
public class AstArrayElementEvaluationTest
{
    [TestCase( "%x", DataTypeFlag.TypeInt )]
    [TestCase( "~x", DataTypeFlag.TypeReal )]
    [TestCase( "!x", DataTypeFlag.TypeString )]
    public void ArrayElementTest( string variableName, DataTypeFlag expectedType )
    {
        // declare %x[10] := (0,.....)
        // %x[10]

        var compilerMessageManger = ICompilerMessageManger.Default;
        var symbolTable = MockUtility.CreateAggregateSymbolTable();
        var variable = MockUtility.CreateVariable( variableName );

        variable.ArraySize = 10;
        variable.State     = SymbolState.Initialized;
        symbolTable.Variables.Add( variable );

        var expr = new AstArrayElementExpressionNode
        {
            Left = new AstSymbolExpressionNode( variable.Name, NullAstExpressionNode.Instance ),
            Right = new AstIntLiteralNode( 0 )
        };

        var visitor = new MockArrayElementEvaluatorVisitor();
        var evaluator = new ArrayElementEvaluator( compilerMessageManger, symbolTable.Variables );

        visitor.Inject( evaluator );
        var result = (AstExpressionNode)visitor.Visit( expr );

        compilerMessageManger.WriteTo( Console.Out );

        ClassicAssert.AreEqual( 0, compilerMessageManger.Count( CompilerMessageLevel.Error ) );
    }

    [TestCase( 1, 2 )]
    [TestCase( 2, -1 )]
    public void ArrayOutOfBoundsTest( int arraySize, int arrayIndex )
    {
        // declare %x[arraySize] := (0,.....)
        // %x[arrayIndex] <-- out of bounds

        var compilerMessageManger = ICompilerMessageManger.Default;
        var symbolTable = MockUtility.CreateAggregateSymbolTable();
        var variable = MockUtility.CreateVariable( "%x" );

        variable.ArraySize = arraySize;
        variable.State     = SymbolState.Initialized;
        symbolTable.Variables.Add( variable );

        var expr = new AstArrayElementExpressionNode
        {
            Left  = new AstSymbolExpressionNode( variable.Name, NullAstExpressionNode.Instance ),
            Right = new AstIntLiteralNode( arrayIndex )
        };

        var visitor = new MockArrayElementEvaluatorVisitor();
        var evaluator = new ArrayElementEvaluator( compilerMessageManger, symbolTable.Variables );

        visitor.Inject( evaluator );
        visitor.Visit( expr );

        compilerMessageManger.WriteTo( Console.Out );

        ClassicAssert.AreEqual( 1, compilerMessageManger.Count( CompilerMessageLevel.Error ) );

    }
}
