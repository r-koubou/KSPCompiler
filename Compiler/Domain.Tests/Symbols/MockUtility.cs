using System.Collections.Generic;

using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Domain.Tests.Symbols;

internal static class MockUtility
{
    public static CallbackSymbol CreateCallbackSymbol( SymbolName name )
        => new CallbackSymbol( true )
        {
            Name = name
        };

    public static CallbackArgumentSymbol CreateArgumentSymbol( string name, bool requiredDeclareOnInit = true )
        => new CallbackArgumentSymbol( requiredDeclareOnInit )
        {
            Name = new SymbolName( name )
        };
    public static CallbackArgumentSymbolList CreateArgumentSymbolList( params string[] names )
    {
        var result = new CallbackArgumentSymbolList();

        foreach ( var name in names )
        {
            result.Add( CreateArgumentSymbol( name ) );
        }

        return result;
    }

    public static CallbackSymbol CreateCallbackSymbol( string name )
        => new CallbackSymbol( true )
        {
            Name = new SymbolName( name )
        };

    public static CallbackSymbol CreateCallbackSymbol( string name, params string[] arguments )
    {
        var symbol = CreateCallbackSymbol( name );
        foreach ( var arg in arguments )
        {
            symbol.Arguments.Add( CreateArgumentSymbol( arg ) );
        }

        return symbol;
    }
}
