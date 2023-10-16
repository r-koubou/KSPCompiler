using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Domain.Tests;

public static class MockSymbolTableUtility
{
    public static VariableSymbolTable CreateDummy()
    {
        var table = new VariableSymbolTable();
        var example = new VariableSymbol
        {
            Name = "$example1",
            Description = "example1 symbol",
            Reserved = true,
        };

        table.Add( example );

        return table;
    }
}
