using System.Text;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;

using NUnit.Framework;

namespace KSPCompiler.Features.SymbolManagement.UseCase.Tests.Analysis.Obfuscators;

[TestFixture]
public class CommandSymbolEvaluatorTest
{
    [Test]
    public void CommandSymbolTest()
    {
        const string commandName = "message";

        var output = new StringBuilder();
        var symbolTable = new AggregateSymbolTable();
        var commandSymbol = MockUtility.CreateCommand(
            commandName,
            DataTypeFlag.TypeVoid,
            new CommandArgumentSymbol
            {
                Name     = "arg",
                DataType = DataTypeFlag.All
            }
        );

        symbolTable.Commands.AddAsOverload( commandSymbol, commandSymbol.Arguments );

        var obfuscatedTable = MockUtility.CreateAggregateObfuscatedSymbolTable( symbolTable, variablePrefix: "v" );

        var expr = new AstSymbolExpressionNode( commandName );
        var evaluator = new SymbolEvaluator( output, symbolTable, obfuscatedTable );
        var visitor = new MockSymbolEvaluatorVisitor();

        visitor.Inject( evaluator );
        visitor.Visit( expr );

        Assert.That( output.ToString(), Is.EqualTo( commandName ) );
    }
}
