using System.Text;

using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Interactors.Analysis.Obfuscators;

using NUnit.Framework;

namespace KSPCompiler.Interactors.Tests.Analysis.Obfuscators;

[TestFixture]
public class VariableSymbolEvaluatorTest
{
    [Test]
    public void UserVariableSymbolTest()
    {
        const string variableName = "$x";
        const string obfuscatedName = "$v0";

        var output = new StringBuilder();
        var symbolTable = MockUtility.CreateAggregateSymbolTable();
        var variable = MockUtility.CreateIntVariable( variableName );

        symbolTable.UserVariables.Add( variable );

        var obfuscatedTable = new ObfuscatedVariableSymbolTable( symbolTable.UserVariables, "v" );

        var expr = new AstSymbolExpressionNode( variableName );
        var evaluator = new SymbolEvaluator( output, symbolTable, obfuscatedTable );
        var visitor = new MockSymbolEvaluatorVisitor();

        visitor.Inject( evaluator );
        visitor.Visit( expr );

        Assert.That( output.ToString(), Is.EqualTo( obfuscatedName ) );
    }

    [Test]
    public void BuiltInVariableSymbolCannotObfuscatedTest()
    {
        // Built-in variables are not obfuscated

        const string variableName = "$ENGINE_PAR_DEMO";

        var output = new StringBuilder();
        var symbolTable = MockUtility.CreateAggregateSymbolTable();
        var variable = MockUtility.CreateIntVariable( variableName );

        variable.BuiltIn = true;
        symbolTable.UserVariables.Add( variable );

        var obfuscatedTable = new ObfuscatedVariableSymbolTable( symbolTable.UserVariables, "v" );

        var expr = new AstSymbolExpressionNode( variableName );
        var evaluator = new SymbolEvaluator( output, symbolTable, obfuscatedTable );
        var visitor = new MockSymbolEvaluatorVisitor();

        visitor.Inject( evaluator );
        visitor.Visit( expr );

        Assert.That( output.ToString(), Is.EqualTo( variableName ) );
    }
}
