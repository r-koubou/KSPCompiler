using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Domain.Symbols;

/// <summary>
/// Represents a symbol in the symbol table.
/// </summary>
/// <seealso cref="ISymbolTable{TSymbol}"/>
public abstract class SymbolBase
{
    /// <summary>
    /// A symbol's name
    /// </summary>
    public SymbolName Name { get; set; } = SymbolName.Empty;

    /// <summary>
    /// Reserved symbol in external (e.g. KSP Built-in, Reserved name by NI etc.)
    /// </summary>
    public bool Reserved { get; set; } = false;

    /// <summary>
    /// A symbol's type
    /// </summary>
    public abstract SymbolType Type { get; }

    /// <summary>
    /// A symbol's data type
    /// </summary>
    public DataTypeFlag DataType { get; set; } = DataTypeFlag.None;

    /// <summary>
    /// A symbol's data modifier
    /// </summary>
    public DataTypeModifierFlag DataTypeModifier { get; set; } = DataTypeModifierFlag.None;

    /// <summary>
    /// Index number when stored in <see cref="ISymbolTable{TSymbol}"/>
    /// </summary>
    public UniqueSymbolIndex TableIndex { get; set; } = UniqueSymbolIndex.Zero;
}
