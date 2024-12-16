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
    /// Built-In symbol in external.
    /// </summary>
    public bool BuiltIn { get; set; } = false;

    /// <summary>
    /// A symbol's state for evaluation in analysis.
    /// </summary>
    public SymbolState State { get; set; } = SymbolState.UnInitialized;

    /// <summary>
    /// A symbol's type
    /// </summary>
    public abstract SymbolType Type { get; }

    /// <summary>
    /// A symbol's modifier
    /// </summary>
    public ModifierFlag Modifier { get; set; } = ModifierFlag.None;

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
    /// A version built into KONTAKT.
    /// </summary>
    /// <remarks>
    /// Empty if unknown.
    /// </remarks>
    public SymbolBuiltIntoVersion BuiltIntoVersion { get; set; } = SymbolBuiltIntoVersion.NotAvailable;

    /// <summary>
    /// If the symbol can represent a constant value, this property holds the value. Otherwise, it is null.
    /// </summary>
    public object? ConstantValue { get; set; } = null;
}
