using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Domain.Tests;

public static class MockSymbolTableUtility
{
    public static VariableSymbolTable CreateDummyVariableSymbolTable()
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

    public static CommandSymbolTable CreateDummyCommandSymbolTable()
    {
        var table = new CommandSymbolTable();
        var example = new CommandSymbol()
        {
            Name        = "message",
            Description = "display text in the status line of KONTAKT",
            Reserved    = true,
            DataType    = DataTypeFlag.TypeVoid
        };

        example.AddArgument( new VariableSymbol
        {
            Name = "text",
            DataType = DataTypeFlag.MultipleType,
            Description = "message text",
            Reserved = false,
        });

        table.Add( example );

        return table;
    }
}
