using System.Text;

using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Interactors.Analysis.Obfuscators;
using KSPCompiler.Interactors.Analysis.Obfuscators.Extensions;

using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace KSPCompiler.Interactors.Tests.Analysis.Obfuscators;

[TestFixture]
public class UserFunctionDeclarationTest
{
    [Test]
    public void Test()
    {
        const string functionName = "x";
        const string obfuscatedName = "f0";
        var expected = new StringBuilder()
                      .Append( $"function {obfuscatedName}" )
                      .NewLine()
                      .Append( "end function" )
                      .NewLine().ToString();

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

        ClassicAssert.AreEqual( expected, output.ToString() );
    }
}
