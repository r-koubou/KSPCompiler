using System;

using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.CompilerMessages.Extensions;
using KSPCompiler.Domain.Events;
using KSPCompiler.Domain.Events.Extensions;
using KSPCompiler.Interactors.Analysis.Semantics;
using KSPCompiler.Interactors.Tests.Commons;

using NUnit.Framework;

namespace KSPCompiler.Interactors.Tests.Analysis.Semantics;

[TestFixture]
public class AstCallCommandEvaluationTest
{
    [Test]
    public void CallCommandTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

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

        var evaluator = new CallCommandEvaluator( eventEmitter, symbols.BuiltInVariables, symbols.Commands, symbols.UITypes );
        var visitor = new MockCallCommandExpressionVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, callCommandAst );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 0 ) );
    }

    [Test]
    public void CannotIncompatibleArgCountTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var symbols = MockUtility.CreateAggregateSymbolTable();

        // register command `play_note`
        // play_note(<note-number>, <velocity>, <sample-offset>, <duration>)
        // https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/general-commands#play_note--
        var command = MockUtility.CreatePlayNoteCommand();

        // register the command
        symbols.Commands.Add( command );

        // Create a call command expression node
        // play_note( 40, 100, 0 ) // missing 1 argument (Expected 4 arguments)
        var callCommandAst = MockUtility.CreateCommandExpressionNode(
            "play_note",
            new AstIntLiteralNode( 40 ),
            new AstIntLiteralNode( 100 ),
            new AstIntLiteralNode( 0 )
        );

        var evaluator = new CallCommandEvaluator( eventEmitter, symbols.BuiltInVariables, symbols.Commands, symbols.UITypes );
        var visitor = new MockCallCommandExpressionVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, callCommandAst );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 1 ) );
    }

    [Test]
    public void CannotIncompatibleArgTypeTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var symbols = MockUtility.CreateAggregateSymbolTable();

        // register command `play_note`
        // play_note(<note-number>, <velocity>, <sample-offset>, <duration>)
        // https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/general-commands#play_note--
        var command = MockUtility.CreatePlayNoteCommand();

        // register the command
        symbols.Commands.Add( command );

        // Create a call command expression node
        // play_note( "40", 100, 0, 0 ) // incompatible type (Expected int, but got string)
        var callCommandAst = MockUtility.CreateCommandExpressionNode(
            "play_note",
            new AstStringLiteralNode( "40" ),
            new AstIntLiteralNode( 100 ),
            new AstIntLiteralNode( 0 ),
            new AstIntLiteralNode( 0 )
        );

        var evaluator = new CallCommandEvaluator( eventEmitter, symbols.BuiltInVariables, symbols.Commands, symbols.UITypes );
        var visitor = new MockCallCommandExpressionVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, callCommandAst );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 1 ) );
    }

    [Test]
    public void WarningIfCommandNotRegisteredTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationWarningEvent>( e => compilerMessageManger.Warning( e.Position, e.Message ) );

        var symbols = MockUtility.CreateAggregateSymbolTable();

        // Don't register the command for make a warning
        // var command = MockUtility.CreatePlayNoteCommand();
        // symbols.Commands.Add( command );

        // Creation of a non-existent call command expression node
        var callCommandAst = MockUtility.CreateCommandExpressionNode(
            "play_note",
            new AstIntLiteralNode( 40 ),
            new AstIntLiteralNode( 100 ),
            new AstIntLiteralNode( 0 ),
            new AstIntLiteralNode( 0 )
        );

        var evaluator = new CallCommandEvaluator( eventEmitter, symbols.BuiltInVariables, symbols.Commands, symbols.UITypes );
        var visitor = new MockCallCommandExpressionVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, callCommandAst );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Warning ), Is.EqualTo( 1 ) );
    }
}
