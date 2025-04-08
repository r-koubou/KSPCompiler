using System.Text;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators.Extensions;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;
using KSPCompiler.Shared.Domain.Compilation.Symbols;

using NUnit.Framework;

namespace KSPCompiler.Features.SymbolManagement.UseCase.Tests.Analysis.Obfuscators;

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
        var symbolTable = new AggregateSymbolTable();
        var variable = MockUtility.CreateIntVariable( variableName );

        variable.State = SymbolState.Initialized;
        symbolTable.UserVariables.Add( variable );

        var variableNode = MockUtility.CreateSymbolNode( variableName );

        var command = MockUtility.CreateMessageCommand();
        symbolTable.Commands.AddAsOverload( command, command.Arguments );

        var obfuscatedTable = new ObfuscatedVariableSymbolTable( symbolTable.UserVariables, "v" );

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
