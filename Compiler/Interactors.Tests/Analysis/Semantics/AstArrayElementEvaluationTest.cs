using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.CompilerMessages.Extensions;
using KSPCompiler.Domain.Events;
using KSPCompiler.Domain.Events.Extensions;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Interactors.Analysis.Semantics;
using KSPCompiler.Interactors.Tests.Commons;

using NUnit.Framework;

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
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var symbolTable = MockUtility.CreateAggregateSymbolTable();
        var variable = MockUtility.CreateVariable( variableName );

        variable.ArraySize = 10;
        variable.State     = SymbolState.Initialized;
        symbolTable.UserVariables.Add( variable );

        var expr = new AstArrayElementExpressionNode
        {
            Left = new AstSymbolExpressionNode( variable.Name, NullAstExpressionNode.Instance ),
            Right = new AstIntLiteralNode( 0 )
        };

        var visitor = new MockArrayElementEvaluatorVisitor();
        var evaluator = new ArrayElementEvaluator( eventEmitter, symbolTable.UserVariables );

        visitor.Inject( evaluator );
        var result = (AstExpressionNode)visitor.Visit( expr );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 0 ) );
    }

    [TestCase( 1, 2 )]
    [TestCase( 2, -1 )]
    public void ArrayOutOfBoundsTest( int arraySize, int arrayIndex )
    {
        // declare %x[arraySize] := (0,.....)
        // %x[arrayIndex] <-- out of bounds

        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var symbolTable = MockUtility.CreateAggregateSymbolTable();
        var variable = MockUtility.CreateVariable( "%x" );

        variable.ArraySize = arraySize;
        variable.State     = SymbolState.Initialized;
        symbolTable.UserVariables.Add( variable );

        var expr = new AstArrayElementExpressionNode
        {
            Left  = new AstSymbolExpressionNode( variable.Name, NullAstExpressionNode.Instance ),
            Right = new AstIntLiteralNode( arrayIndex )
        };

        var visitor = new MockArrayElementEvaluatorVisitor();
        var evaluator = new ArrayElementEvaluator( eventEmitter, symbolTable.UserVariables );

        visitor.Inject( evaluator );
        visitor.Visit( expr );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 1 ) );
    }
}
