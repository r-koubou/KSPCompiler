using System.Text;

using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Interactors.Analysis.Obfuscators;

using NUnit.Framework;
using NUnit.Framework.Legacy;

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

        symbolTable.Variables.Add( variable );

        var obfuscatedTable = new ObfuscatedVariableSymbolTable( symbolTable.Variables, "v" );

        var expr = new AstSymbolExpressionNode( variableName );
        var evaluator = new SymbolEvaluator( output, symbolTable, obfuscatedTable );
        var visitor = new MockSymbolEvaluatorVisitor();

        visitor.Inject( evaluator );
        visitor.Visit( expr );

        ClassicAssert.AreEqual( obfuscatedName, output.ToString() );
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
        symbolTable.Variables.Add( variable );

        var obfuscatedTable = new ObfuscatedVariableSymbolTable( symbolTable.Variables, "v" );

        var expr = new AstSymbolExpressionNode( variableName );
        var evaluator = new SymbolEvaluator( output, symbolTable, obfuscatedTable );
        var visitor = new MockSymbolEvaluatorVisitor();

        visitor.Inject( evaluator );
        visitor.Visit( expr );

        ClassicAssert.AreEqual( variableName, output.ToString() );
    }
}
