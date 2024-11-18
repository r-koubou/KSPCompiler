using System.Text;

using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Interactors.Analysis.Obfuscators;

using NUnit.Framework;

namespace KSPCompiler.Interactors.Tests.Analysis.Obfuscators;

[TestFixture]
public class UserFunctionDeclarationTest
{
    [Test]
    public void Test()
    {
        const string functionName = "x";
        const string obfuscatedName = "f0";
        const string expected = $"function {obfuscatedName}\nend function\n";

        var output = new StringBuilder();
        var symbolTable = MockUtility.CreateAggregateSymbolTable();

        symbolTable.UserFunctions.Add( MockUtility.CreateUserFunction( functionName ) );

        var obfuscatedTable = new ObfuscatedUserFunctionSymbolTable( symbolTable.UserFunctions, "f" );

        var node = new AstUserFunctionDeclarationNode
        {
            Name = functionName
        };

        var evaluator = new UserFunctionDeclarationEvaluator( output, obfuscatedTable );
        var visitor = new MockUserFunctionDeclarationVisitor( output );

        visitor.Inject( evaluator );
        visitor.Visit( node );

        Assert.AreEqual( expected, output.ToString() );
    }
}
