using System.Text;

using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Interactors.Analysis.Obfuscators;

using NUnit.Framework;

namespace KSPCompiler.Interactors.Tests.Analysis.Obfuscators;

[TestFixture]
public class CallCommandTest
{
    [Test]
    public void Test()
    {
        // message( $x )
        // => message( $v0 )

        const string variableName = "$x";
        const string obfuscatedName = "$v0";

        var output = new StringBuilder();
        var symbolTable = MockUtility.CreateAggregateSymbolTable();
        var variable = MockUtility.CreateIntVariable( variableName );

        variable.State = SymbolState.Initialized;
        symbolTable.Variables.Add( variable );

        var variableNode = MockUtility.CreateSymbolNode( variableName );

        var command = MockUtility.CreateMessageCommand();
        symbolTable.Commands.Add( command );

        var obfuscatedTable = new ObfuscatedVariableSymbolTable( symbolTable.Variables, "v" );

        var node = new AstCallCommandExpressionNode
        {
            Left = MockUtility.CreateSymbolNode( "message" ),
            Right = new AstExpressionListNode
            {
                Expressions = { variableNode }
            }
        };

        var evaluator = new CallCommandEvaluator( output );
        var visitor = new MockCallCommandEvaluatorVisitor( output, obfuscatedTable );

        visitor.Inject( evaluator );
        visitor.Visit( node );

        Assert.AreEqual( $"message({obfuscatedName})", output.ToString() );
    }
}
