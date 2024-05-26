using System.Collections.Generic;

using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Domain.Tests;

public static class MockSymbolTableUtility
{
    public static List<VariableSymbol> CreateDummyVariableSymbols()
    {
        var list = new List<VariableSymbol>();
        var example = new VariableSymbol
        {
            Name = "$example1",
            Description = "example1 symbol",
            Reserved = true,
        };

        list.Add( example );

        return list;
    }

    public static List<CommandSymbol> CreateDummyCommandSymbols()
    {
        var list = new List<CommandSymbol>();
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

        list.Add( example );

        return list;
    }

    public static List<UITypeSymbol> CreateDummyUITypeSymbols()
    {
        var list = new List<UITypeSymbol>();
        var example = new UITypeSymbol(false)
        {
            Name        = "ui_button",
            Description = "Button",
            Reserved    = true,
            DataType    = DataTypeFlag.TypeInt
        };

        example.AddInitializerArgument( new VariableSymbol
        {
            Name        = "width",
            DataType    = DataTypeFlag.TypeInt,
            Description = "button width",
            Reserved    = false,
        });

        list.Add( example );

        return list;
    }

}
