using System.Text;

using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Interactors.Analysis.Obfuscators;
using KSPCompiler.Interactors.Analysis.Obfuscators.Extensions;

using NUnit.Framework;

namespace KSPCompiler.Interactors.Tests.Analysis.Obfuscators;

[TestFixture]
public class AssignOperatorTest
{
    [Test]
    public void Test()
    {
        // $x := 5
        // => $v0 := 5
        //
        // - := のノード評価で左辺と右辺はそれぞれ式の評価で出力される
        //   ＆それぞれの単体テストでテストするのでここでは扱わない
        // - （:= 単体でのテストの確認となる）

        const string variableName = "$x";
        const string obfuscatedName = "$v0";
        var expected = new StringBuilder()
                      .Append( $"{obfuscatedName} := " )
                      .Append( 5 )
                      .NewLine().ToString();

        var output = new StringBuilder();
        var symbolTable = MockUtility.CreateAggregateSymbolTable();
        var variable = MockUtility.CreateIntVariable( variableName );

        variable.State = SymbolState.Initialized;
        symbolTable.BuiltInVariables.Add( variable );

        var variableNode = MockUtility.CreateSymbolNode( variableName );

        var command = MockUtility.CreateMessageCommand();
        symbolTable.Commands.Add( command );

        var obfuscatedTable = new ObfuscatedVariableSymbolTable( symbolTable.BuiltInVariables, "v" );

        var node = new AstAssignmentExpressionNode
        {
            Left = variableNode,
            Right = new AstIntLiteralNode( 5 )
        };

        var evaluator = new AssignOperatorEvaluator( output );
        var visitor = new MockAssignOperatorEvaluatorVisitor( output, obfuscatedTable );

        visitor.Inject( evaluator );
        visitor.Visit( node );

        Assert.That( output.ToString(), Is.EqualTo( expected ) );
    }
}
