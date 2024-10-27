using System;

using KSPCompiler.Domain.Ast.Analyzers.Semantics;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;

using NUnit.Framework;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

[TestFixture]
public class AstCallCommandEvaluationTest
{
    [Test]
    public void CallCommandTest()
    {
        const string name = "command";

        var compilerMessageManger = ICompilerMessageManger.Default;
        var symbols = MockUtility.CreateAggregateSymbolTable();

        // register command `play_note`
        // play_note(<note-number>, <velocity>, <sample-offset>, <duration>)
        // https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/general-commands#play_note--
        var command = MockUtility.CreatePlayNoteCommand();

        // register the command
        symbols.Commands.Add( command );

        // Create a call command expression node
        // play_note( 40, 100, 0, 0 )
        var callCommandAst = MockUtility.CreateCommandExpressionNode(
            "play_note",
            new AstIntLiteralNode( 40 ),
            new AstIntLiteralNode( 100 ),
            new AstIntLiteralNode( 0 ),
            new AstIntLiteralNode( 0 )
        );

        var evaluator = new CallCommandExpressionEvaluator( compilerMessageManger, symbols.Commands );
        var visitor = new MockCallCommandExpressionVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, callCommandAst );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.AreEqual( 0, compilerMessageManger.Count( CompilerMessageLevel.Error ) );
    }
}
