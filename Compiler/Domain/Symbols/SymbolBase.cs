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
    /// This symbol is referenced in the script.
    /// </summary>
    public bool Referenced { get; set; } = false;

    /// <summary>
    /// A symbol's type
    /// </summary>
    public abstract SymbolType Type { get; }

    /// <summary>
    /// A symbol's data type
    /// </summary>
    public virtual DataTypeFlag DataType { get; set; } = DataTypeFlag.None;

    /// <summary>
    /// A symbol's data modifier
    /// </summary>
    public DataTypeModifierFlag DataTypeModifier { get; set; } = DataTypeModifierFlag.None;

    /// <summary>
    /// Index number when stored in <see cref="ISymbolTable{TSymbol}"/>
    /// </summary>
    public UniqueSymbolIndex TableIndex { get; set; } = UniqueSymbolIndex.Null;

    /// <summary>
    /// Symbol's specified description.
    /// </summary>
    /// <remarks>
    /// Empty characters are also acceptable since they correspond to document comments.
    /// </remarks>
    public SymbolDescription Description { get; set; } = SymbolDescription.Empty;

    /// <summary>
    /// If the symbol can represent a value, this property holds the value. (e.g. constant value). Otherwise, it is null.
    /// </summary>
    public object? Value { get; set; } = null;
}
