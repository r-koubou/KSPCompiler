using System.Text;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators;
using KSPCompiler.Shared.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Shared.Domain.Symbols;
using KSPCompiler.Shared.Domain.Symbols.MetaData;

using NUnit.Framework;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Obfuscators;

[TestFixture]
public class CommandSymbolEvaluatorTest
{
    [Test]
    public void CommandSymbolTest()
    {
        const string commandName = "message";

        var output = new StringBuilder();
        var symbolTable = MockUtility.CreateAggregateSymbolTable();
        var commandSymbol = MockUtility.CreateCommand(
            commandName,
            DataTypeFlag.TypeVoid,
            new CommandArgumentSymbol
            {
                Name     = "arg",
                DataType = DataTypeFlag.All
            }
        );

        symbolTable.Commands.Add( commandSymbol );

        var obfuscatedTable = MockUtility.CreateAggregateObfuscatedSymbolTable( symbolTable, variablePrefix: "v" );

        var expr = new AstSymbolExpressionNode( commandName );
        var evaluator = new SymbolEvaluator( output, symbolTable, obfuscatedTable );
        var visitor = new MockSymbolEvaluatorVisitor();

        visitor.Inject( evaluator );
        visitor.Visit( expr );

        Assert.That( output.ToString(), Is.EqualTo( commandName ) );
    }
}
