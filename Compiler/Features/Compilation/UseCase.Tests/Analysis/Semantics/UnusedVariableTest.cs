using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.Compilation.Domain.Messages;
using KSPCompiler.Features.Compilation.Domain.Messages.Extensions;
using KSPCompiler.Features.Compilation.Gateways.EventEmitting;
using KSPCompiler.Features.Compilation.UseCase.Analysis;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions;
using KSPCompiler.Features.SymbolManagement.UseCase.Tests.Commons;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Blocks;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Statements;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.EventEmitting.Extensions;
using KSPCompiler.Shared.Text;

using NUnit.Framework;

namespace KSPCompiler.Features.SymbolManagement.UseCase.Tests.Analysis.Semantics;

[TestFixture]
public class UnusedVariableTest
{
    [Test]
    public async Task WarnIfVariableIsNotUsedTest()
    {
        const string name = "$variable";

        // Construct the ast tree
        /*
             on init
                declare $variable := 0 {declared but not used}
             end on
         */
        var rootAst = new AstCompilationUnitNode();
        var callbackAst = MockUtility.CreateCallbackDeclarationNode( "init" );

        // declare $variable := 0
        var declaration = MockUtility.CreateVariableDeclarationNode( name );
        declaration.Position = new Position
        {
            BeginLine = 1,
            BeginColumn = 0
        };
        declaration.Initializer = new AstVariableInitializerNode( declaration )
        {
            PrimitiveInitializer = new AstPrimitiveInitializerNode(
                declaration,
                new AstIntLiteralNode( 0 ),
                NullAstExpressionListNode.Instance
            )
        };

        declaration.Parent = callbackAst;

        callbackAst.Block.Statements.Add( declaration );
        rootAst.GlobalBlocks.Add( callbackAst );

        // Prepare the event emitter, receiver
        var compilerMessageManger = ICompilerMessageManger.Default;
        var eventEmitter = new MockEventEmitter();
        eventEmitter.Subscribe<CompilationInfoEvent>(
            e
                => compilerMessageManger.Info( e.Position, e.Message )
        );

        var symbols = new AggregateSymbolTable();
        // Register init callback as builtin-symbol
        symbols.BuiltInCallbacks.AddAsNoOverload( new CallbackSymbol( false )
            {
                Name = "init",
                BuiltIn = true
            }
        );

        // Run semantic analysis
        var interactor = new SemanticAnalysisInteractor();
        var input = new SemanticAnalysisInputData(
            new SemanticAnalysisInputDataDetail(
                eventEmitter,
                rootAst,
                symbols
            )
        );

        var result = await interactor.ExecuteAsync( input, CancellationToken.None );
        Assert.That( result.Result, Is.True, $"Semantic analysis failed : {result.Error}" );

        compilerMessageManger.WriteTo( Console.Out );

        // Expect the variable to be marked as unused
        if( symbols.UserVariables.TrySearchByName( name, out var variableSymbol ) )
        {
            Assert.That( variableSymbol.State, Is.EqualTo( SymbolState.Initialized ) );
        }
        else
        {
            Assert.Fail( "Variable symbol not found." );
        }

        // Expect a warning for unused variable
        Assert.That( compilerMessageManger.Count( CompilerMessageLevel.Info ), Is.EqualTo( 1 ) );

    }
}
