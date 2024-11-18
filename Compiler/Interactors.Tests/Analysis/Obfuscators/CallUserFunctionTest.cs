using System.Text;

using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Interactors.Analysis.Obfuscators;

using NUnit.Framework;

namespace KSPCompiler.Interactors.Tests.Analysis.Obfuscators;

[TestFixture]
public class CallUserFunctionTest
{
    [Test]
    public void Test()
    {
        const string functionName = "x";
        const string obfuscatedName = "f0";
        const string expected = $"call {obfuscatedName}\n";

        var output = new StringBuilder();
        var symbolTable = MockUtility.CreateAggregateSymbolTable();

        symbolTable.UserFunctions.Add( MockUtility.CreateUserFunction( functionName ) );

        var obfuscatedTable = new ObfuscatedUserFunctionSymbolTable( symbolTable.UserFunctions, "f" );

        var node = new AstCallUserFunctionStatementNode
        {
            Name = functionName
        };

        var evaluator = new CallUserFunctionEvaluator( output, obfuscatedTable );
        var visitor = new MockCallUserFunctionVisitor( output );

        visitor.Inject( evaluator );
        visitor.Visit( node );

        Assert.AreEqual( expected, output.ToString() );
    }
}
