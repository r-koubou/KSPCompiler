using System.Text;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators.Extensions;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Blocks;
using KSPCompiler.Shared.Domain.Compilation.Symbols;

using NUnit.Framework;

namespace KSPCompiler.Features.SymbolManagement.UseCase.Tests.Analysis.Obfuscators;

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
        var symbolTable = new AggregateSymbolTable();

        symbolTable.UserFunctions.Add( MockUtility.CreateUserFunction( functionName ) );

        var obfuscatedTable = MockUtility.CreateAggregateObfuscatedSymbolTable( symbolTable, functionPrefix: "f" );

        var node = new AstUserFunctionDeclarationNode
        {
            Name = functionName
        };

        var evaluator = new UserFunctionDeclarationEvaluator( output, obfuscatedTable );
        var visitor = new MockUserFunctionDeclarationVisitor( output );

        visitor.Inject( evaluator );
        visitor.Visit( node );

        Assert.That( output.ToString(), Is.EqualTo( expected ) );
    }
}
