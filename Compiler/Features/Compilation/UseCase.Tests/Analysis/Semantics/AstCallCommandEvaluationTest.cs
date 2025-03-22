using System;

using KSPCompiler.Features.Compilation.Domain.Messages;
using KSPCompiler.Features.Compilation.Domain.Messages.Extensions;
using KSPCompiler.Features.Compilation.Gateways.EventEmitting;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Semantics;
using KSPCompiler.Features.SymbolManagement.UseCase.Tests.Commons;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.EventEmitting.Extensions;

using NUnit.Framework;

namespace KSPCompiler.Features.SymbolManagement.UseCase.Tests.Analysis.Semantics;

[TestFixture]
public class AstCallCommandEvaluationTest
{
    [Test]
    public void CallCommandTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var symbols = new AggregateSymbolTable();

        // register command `play_note`
        // play_note(<note-number>, <velocity>, <sample-offset>, <duration>)
        // https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/general-commands#play_note--
        var command = MockUtility.CreatePlayNoteCommand();

        // register the command
        symbols.Commands.AddAsOverload( command, command.Arguments );

        // Create a call command expression node
        // play_note( 40, 100, 0, 0 )
        var callCommandAst = MockUtility.CreateCommandExpressionNode(
            "play_note",
            new AstIntLiteralNode( 40 ),
            new AstIntLiteralNode( 100 ),
            new AstIntLiteralNode( 0 ),
            new AstIntLiteralNode( 0 )
        );

        var evaluator = new CallCommandEvaluator( eventEmitter, symbols );
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

        var symbols = new AggregateSymbolTable();

        // register command `play_note`
        // play_note(<note-number>, <velocity>, <sample-offset>, <duration>)
        // https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/general-commands#play_note--
        var command = MockUtility.CreatePlayNoteCommand();

        // register the command
        symbols.Commands.AddAsOverload( command, command.Arguments );

        // Create a call command expression node
        // play_note( 40, 100, 0 ) // missing 1 argument (Expected 4 arguments)
        var callCommandAst = MockUtility.CreateCommandExpressionNode(
            "play_note",
            new AstIntLiteralNode( 40 ),
            new AstIntLiteralNode( 100 ),
            new AstIntLiteralNode( 0 )
        );

        var evaluator = new CallCommandEvaluator( eventEmitter, symbols );
        var visitor = new MockCallCommandExpressionVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, callCommandAst );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 1 ) );
    }

    [Test]
    public void CannotIncompatibleArgCountWithOverloadedTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var symbols = new AggregateSymbolTable();

        // register command `note_off`
        // note_off(<event-id>)
        // note_off(<event-id>, <time-offset>)
        // https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/general-commands#note_off--
        var commands = MockUtility.CreateNoteOffCommand();

        // register the command
        foreach( var x in commands )
        {
            symbols.Commands.AddAsOverload( x, x.Arguments );
        }

        // Create a call command expression node
        // note_off() // missing argument(s)
        // [Expected]
        // note_off(<event-id>)
        // note_off(<event-id>, <time-offset>)
        var callCommandAst = MockUtility.CreateCommandExpressionNode(
            "note_off"
        );

        var evaluator = new CallCommandEvaluator( eventEmitter, symbols );
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

        var symbols = new AggregateSymbolTable();

        // register command `play_note`
        // play_note(<note-number>, <velocity>, <sample-offset>, <duration>)
        // https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/general-commands#play_note--
        var command = MockUtility.CreatePlayNoteCommand();

        // register the command
        symbols.Commands.AddAsOverload( command, command.Arguments );

        // Create a call command expression node
        // play_note( "40", 100, 0, 0 ) // incompatible type (Expected int, but got string)
        var callCommandAst = MockUtility.CreateCommandExpressionNode(
            "play_note",
            new AstStringLiteralNode( "40" ),
            new AstIntLiteralNode( 100 ),
            new AstIntLiteralNode( 0 ),
            new AstIntLiteralNode( 0 )
        );

        var evaluator = new CallCommandEvaluator( eventEmitter, symbols );
        var visitor = new MockCallCommandExpressionVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, callCommandAst );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Error ), Is.EqualTo( 1 ) );
    }

    [Test]
    public void CannotIncompatibleArgTypeWithOverloadedTest()
    {
        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationErrorEvent>( e => compilerMessageManger.Error( e.Position, e.Message ) );

        var symbols = new AggregateSymbolTable();

        // register command `note_off`
        // note_off(<event-id>)
        // note_off(<event-id>, <time-offset>)
        // https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/general-commands#note_off--
        var commands = MockUtility.CreateNoteOffCommand();

        // register the command
        foreach( var x in commands )
        {
            symbols.Commands.AddAsOverload( x, x.Arguments );
        }

        // Create a call command expression node
        // note_off("10") // incompatible type (Expected int, but got string)
        // [Expected]
        // note_off(<event-id>)
        // note_off(<event-id>, <time-offset>)
        var callCommandAst = MockUtility.CreateCommandExpressionNode(
            "note_off",
            new AstStringLiteralNode( "10" )
        );

        var evaluator = new CallCommandEvaluator( eventEmitter, symbols );
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

        var symbols = new AggregateSymbolTable();

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

        var evaluator = new CallCommandEvaluator( eventEmitter, symbols );
        var visitor = new MockCallCommandExpressionVisitor();

        visitor.Inject( evaluator );
        evaluator.Evaluate( visitor, callCommandAst );

        compilerMessageManger.WriteTo( Console.Out );

        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Warning ), Is.EqualTo( 1 ) );
    }
}
