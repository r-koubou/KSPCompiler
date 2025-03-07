using System.Collections.Generic;

using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.Commons.Tests;

public static class MockSymbolTableUtility
{
    public static List<VariableSymbol> CreateDummyVariableSymbols()
    {
        var list = new List<VariableSymbol>();
        var example = new VariableSymbol
        {
            Name        = "$example1",
            Description = "example1 symbol",
            BuiltIn     = true,
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
            BuiltIn    = true,
            DataType    = DataTypeFlag.TypeVoid
        };

        var ui = new [] { "ui_*" };

        example.AddArgument( new CommandArgumentSymbol( ui )
            {
                Name        = "text",
                DataType    = DataTypeFlag.All,
                Description = "message text",
                BuiltIn     = false,
            }
        );

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
            BuiltIn    = true,
            DataType    = DataTypeFlag.TypeInt
        };

        example.AddInitializerArgument( new UIInitializerArgumentSymbol
            {
                Name        = "$width",
                DataType    = DataTypeFlag.TypeInt,
                Description = "button width",
                BuiltIn     = false,
            }
        );

        list.Add( example );

        return list;
    }

    public static List<CallbackSymbol> CreateDummyCallbackSymbols()
    {
        var list = new List<CallbackSymbol>();
        var example = new CallbackSymbol( true )
        {
            Name        = "ui_control",
            Description = "UI event callback",
            BuiltIn    = true
        };

        example.Arguments.Add( new CallbackArgumentSymbol( false )
            {
                Name        = "$button",
                DataType    = DataTypeFlag.TypeInt,
                Description = "button ui variable",
                BuiltIn     = false,
            }
        );

        list.Add( example );

        return list;
    }
}
