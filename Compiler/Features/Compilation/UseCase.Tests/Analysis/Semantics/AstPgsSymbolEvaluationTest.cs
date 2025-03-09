using System;

using KSPCompiler.Features.Compilation.Domain.Messages;
using KSPCompiler.Features.Compilation.Domain.Messages.Extensions;
using KSPCompiler.Features.Compilation.Gateways.EventEmitting;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics;
using KSPCompiler.Features.Compilation.UseCase.Tests.Commons;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;
using KSPCompiler.Shared.EventEmitting.Extensions;

using NUnit.Framework;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Semantics;

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
        var symbolTable = new AggregateSymbolTable();

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
