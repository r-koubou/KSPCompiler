using System.Text;

using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Interactors.Analysis.Obfuscators;
using KSPCompiler.Interactors.Analysis.Obfuscators.Extensions;

using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace KSPCompiler.Interactors.Tests.Analysis.Obfuscators;

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

        var obfuscatedTable = new ObfuscatedUserFunctionSymbolTable( symbolTable.UserFunctions, "f" );

        var node = new AstCallUserFunctionStatementNode
        {
            Name = functionName
        };

        var evaluator = new CallUserFunctionEvaluator( output, obfuscatedTable );
        var visitor = new MockCallUserFunctionVisitor( output );

        visitor.Inject( evaluator );
        visitor.Visit( node );

        ClassicAssert.AreEqual( expected, output.ToString() );
    }
}
