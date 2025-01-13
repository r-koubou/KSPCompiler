using System;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.CompilerMessages.Extensions;
using KSPCompiler.Domain.Events;
using KSPCompiler.Domain.Events.Extensions;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Interactors.Analysis.Semantics;
using KSPCompiler.Interactors.Tests.Commons;

using NUnit.Framework;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

[TestFixture]
public class AstPgsSymbolEvaluationTest
{
    private void PgsSymbolTestBody(
        AstSymbolExpressionNode expr,
        int expectedErrorCount )
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var visitor = new MockAstSymbolVisitor();
        var symbolTable = MockUtility.CreateAggregateSymbolTable();

        var symbolEvaluator = new SymbolEvaluator( eventEmitter, symbolTable );
        visitor.Inject( symbolEvaluator );

        var result = visitor.Visit( expr );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( result,                                                    Is.InstanceOf<AstExpressionNode>() );
        Assert.That( ( (AstExpressionNode)result ).TypeFlag,                    Is.EqualTo( DataTypeFlag.TypePgsId ) );
        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( expectedErrorCount ) );
    }

    [Test]
    public void PgsSymbolEvalTest()
    {
        var symbolExpr = new AstSymbolExpressionNode( "TEST" );
        PgsSymbolTestBody( symbolExpr, 0 );
    }

    [Test]
    public void CannotEvaluatePgsIdIsOver64Characters()
    {
        var over64Chars = new string( 'A', 65 );
        var symbolExpr = new AstSymbolExpressionNode( over64Chars );
        PgsSymbolTestBody( symbolExpr, 1 );
    }
}
