using System;

namespace KSPCompiler.Shared.Domain.Compilation.Symbols;

public sealed class SymbolNotFoundException : Exception
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public SymbolNotFoundException( string message ) : base( message ) {}

    public static SymbolNotFoundException Variable( string name  )
    {
        return new SymbolNotFoundException( $"Variable '{name}' not found" );
    }

    public static SymbolNotFoundException Command( string name  )
    {
        return new SymbolNotFoundException( $"Command '{name}' not found" );
    }

    public static SymbolNotFoundException Callback( string name )
    {
        return new SymbolNotFoundException( $"Callback '{name}' not found" );
    }

    public static SymbolNotFoundException UserFunction( string name )
    {
        return new SymbolNotFoundException( $"User function '{name}' not found" );
    }

    public static SymbolNotFoundException UIType( string name )
    {
        return new SymbolNotFoundException( $"UI type '{name}' not found" );
    }
}
