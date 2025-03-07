using System.Text;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators.Extensions;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Statements;

using NUnit.Framework;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Obfuscators;

[TestFixture]
public class CallUserFunctionTest
{
    [Test]
    public void Test()
    {
        const string functionName = "x";
        const string obfuscatedName = "f0";
        var expected = new StringBuilder()
                      .Append( $"call {obfuscatedName}" )
                      .NewLine().ToString();

        var output = new StringBuilder();
        var symbolTable = MockUtility.CreateAggregateSymbolTable();

        symbolTable.UserFunctions.Add( MockUtility.CreateUserFunction( functionName ) );

        //var obfuscatedTable = new ObfuscatedUserFunctionSymbolTable( symbolTable.UserFunctions, "f" );
        var obfuscatedTable = MockUtility.CreateAggregateObfuscatedSymbolTable( symbolTable, functionPrefix: "f" );

        var node = new AstCallUserFunctionStatementNode
        {
            Symbol = new AstSymbolExpressionNode( functionName )
        };

        var evaluator = new CallUserFunctionEvaluator( output, obfuscatedTable );
        var visitor = new MockCallUserFunctionVisitor( output );

        visitor.Inject( evaluator );
        visitor.Visit( node );

        Assert.That( output.ToString(), Is.EqualTo( expected ) );
    }
}
