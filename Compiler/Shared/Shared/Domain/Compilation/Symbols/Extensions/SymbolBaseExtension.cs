using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData.Extensions;

namespace KSPCompiler.Shared.Domain.Compilation.Symbols.Extensions;

public static class SymbolBaseExtension
{
    /// <summary>
    /// Trying to get the constant value of the symbol.
    /// </summary>
    /// <returns>If the symbol is a constant and the value is of type <typeparamref name="T"/>, the value is returned and true. Otherwise, false.</returns>
    public static bool TryGetConstantValue<T>( this SymbolBase symbol, out T result )
    {
        var symbolValue = symbol.ConstantValue;
        result = default!;

        if( !symbol.Modifier.IsConstant() )
        {
            return false;
        }

        if( symbolValue is not T value )
        {
            return false;
        }

        result = value;

        return true;
    }
}
