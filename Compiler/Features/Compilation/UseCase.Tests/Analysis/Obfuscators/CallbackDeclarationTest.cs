using System.Text;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators.Extensions;
using KSPCompiler.Shared.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Shared.Domain.Symbols;

using NUnit.Framework;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Obfuscators;

[TestFixture]
public class CallbackDeclarationTest
{
    [Test]
    public void NoArgumentCallbackTest()
    {
        const string callbackName = "init";
        var expected = new StringBuilder()
                      .Append( $"on {callbackName}" )
                      .NewLine()
                      .Append( "end on" )
                      .NewLine().ToString();

        var output = new StringBuilder();
        var node = new AstCallbackDeclarationNode
        {
            Name = callbackName
        };

        var symbolTable = MockUtility.CreateAggregateSymbolTable();
        var obfuscatedTable = MockUtility.CreateAggregateObfuscatedSymbolTable( symbolTable, functionPrefix: "v" );

        var evaluator = new CallbackDeclarationEvaluator( output, obfuscatedTable );
        var visitor = new MockCallbackDeclarationVisitor( output );

        visitor.Inject( evaluator );
        visitor.Visit( node );

        Assert.That( output.ToString(), Is.EqualTo( expected ) );
    }

    [Test]
    public void ArgumentCallbackTest()
    {
        const string callbackName = "ui_control";
        const string argumentName = "$v0";

        var expected = new StringBuilder()
                      .Append( $"on {callbackName} ({argumentName})" )
                      .NewLine()
                      .Append( "end on" )
                      .NewLine().ToString();

        var output = new StringBuilder();
        var node = new AstCallbackDeclarationNode
        {
            Name = callbackName,
            ArgumentList = new AstArgumentListNode
            {
                Arguments =
                {
                    new AstArgumentNode
                    {
                        Name = argumentName
                    }
                }
            }
        };

        var symbolTable = MockUtility.CreateAggregateSymbolTable();
        var variable = MockUtility.CreateIntVariable( argumentName );
        variable.State = SymbolState.Loaded;

        symbolTable.UserVariables.Add( variable );
        var obfuscatedTable = MockUtility.CreateAggregateObfuscatedSymbolTable( symbolTable, functionPrefix: "v" );

        var evaluator = new CallbackDeclarationEvaluator( output, obfuscatedTable );
        var visitor = new MockCallbackDeclarationVisitor( output );

        visitor.Inject( evaluator );
        visitor.Visit( node );

        Assert.That( output.ToString(), Is.EqualTo( expected ) );
    }
}
