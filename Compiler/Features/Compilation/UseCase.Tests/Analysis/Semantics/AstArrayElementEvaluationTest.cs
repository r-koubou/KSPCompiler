using System;

using KSPCompiler.Features.Compilation.Domain.Messages;
using KSPCompiler.Features.Compilation.Domain.Messages.Extensions;
using KSPCompiler.Features.Compilation.Gateways.EventEmitting;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics;
using KSPCompiler.Features.SymbolManagement.UseCase.Tests.Commons;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;
using KSPCompiler.Shared.EventEmitting.Extensions;

using NUnit.Framework;

namespace KSPCompiler.Features.SymbolManagement.UseCase.Tests.Analysis.Semantics;

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

        var symbolTable = new AggregateSymbolTable();
        var variable = MockUtility.CreateVariable( variableName );

        variable.ArraySize = 10;
        variable.State     = SymbolState.Initialized;
        symbolTable.UserVariables.Add( variable );

        var expr = new AstArrayElementExpressionNode();
        expr.Left  = new AstSymbolExpressionNode( expr, variable.Name );
        expr.Right = new AstIntLiteralNode( 0 );

        var visitor = new MockArrayElementEvaluatorVisitor();
        var evaluator = new ArrayElementEvaluator( eventEmitter, symbolTable );

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

        var symbolTable = new AggregateSymbolTable();
        var variable = MockUtility.CreateVariable( "%x" );

        variable.ArraySize = arraySize;
        variable.State     = SymbolState.Initialized;
        symbolTable.UserVariables.Add( variable );

        var expr = new AstArrayElementExpressionNode();
        expr.Left  = new AstSymbolExpressionNode( expr, variable.Name );
        expr.Right = new AstIntLiteralNode( arrayIndex );


        var visitor = new MockArrayElementEvaluatorVisitor();
        var evaluator = new ArrayElementEvaluator( eventEmitter, symbolTable );

        visitor.Inject( evaluator );
        visitor.Visit( expr );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 1 ) );
    }
}
