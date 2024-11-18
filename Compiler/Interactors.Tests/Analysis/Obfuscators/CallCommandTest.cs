using System.Text;

using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Interactors.Analysis.Obfuscators;
using KSPCompiler.Interactors.Analysis.Obfuscators.Extensions;

using NUnit.Framework;

namespace KSPCompiler.Interactors.Tests.Analysis.Obfuscators;

[TestFixture]
public class CallCommandTest
{
    [Test]
    public void ExpressionTest()
    {
        // message( $x )
        // => message( $v0 )

        const string variableName = "$x";
        const string obfuscatedName = "$v0";
        const string expected = $"message({obfuscatedName})";

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

        Assert.AreEqual( expected, output.ToString() );
    }

    [Test]
    public void StatementTest()
    {
        // simulate node in block node
        //
        // message( $x )
        // => message( $v0 )\n

        const string variableName = "$x";
        const string obfuscatedName = "$v0";
        var expected = new StringBuilder()
                      .Append( $"message({obfuscatedName})" )
                      .NewLine().ToString();

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

        var blockNode = new AstBlockNode();
        blockNode.Statements.Add( node );
        node.Parent = new AstBlockNode();

        var evaluator = new CallCommandEvaluator( output );
        var visitor = new MockCallCommandEvaluatorVisitor( output, obfuscatedTable );

        visitor.Inject( evaluator );
        visitor.Visit( node );

        Assert.AreEqual( expected, output.ToString() );
    }

}
