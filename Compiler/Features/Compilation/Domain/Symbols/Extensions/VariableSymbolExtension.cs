namespace KSPCompiler.Domain.Symbols.Extensions;

/// <summary>
/// Extension for <see cref="VariableSymbol"/>
/// </summary>
public static class VariableSymbolExtension
{
    /// <summary>
    /// Check if the symbol is Null Object.
    /// </summary>
    public static bool IsNull( this VariableSymbol? self )
        => self is null or NullVariableSymbol;
}
