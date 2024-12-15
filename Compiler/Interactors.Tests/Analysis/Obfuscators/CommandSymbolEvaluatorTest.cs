using System.Text;

using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.Interactors.Analysis.Obfuscators;

using NUnit.Framework;

namespace KSPCompiler.Interactors.Tests.Analysis.Obfuscators;

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
                DataType = DataTypeFlag.MultipleType
            }
        );

        symbolTable.Commands.Add( commandSymbol );

        var obfuscatedTable = new ObfuscatedVariableSymbolTable( symbolTable.UserVariables, "v" );

        var expr = new AstSymbolExpressionNode( commandName );
        var evaluator = new SymbolEvaluator( output, symbolTable, obfuscatedTable );
        var visitor = new MockSymbolEvaluatorVisitor();

        visitor.Inject( evaluator );
        visitor.Visit( expr );

        Assert.That( output.ToString(), Is.EqualTo( commandName ) );
    }
}
