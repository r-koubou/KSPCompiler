using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Domain.Symbols;

public record VariableSymbol : SymbolBase, ISymbolDataTypeProvider
{
    #region Properties
    public override SymbolType Type
        => SymbolType.Variable;

    ///
    /// <inheritdoc />
    ///
    public virtual DataTypeFlag DataType { get; set; } = DataTypeFlag.None;

    /// <summary>
    /// Number of elements for array type
    /// </summary>
    public virtual int ArraySize { get; set; } = -1;

    // TODO
    /// <summary>
    /// UI type when variable type is UI
    /// </summary>
    /// <seealso cref="ModifierFlag"/>
    /// <seealso cref="ModifierFlag.UI"/>
    public virtual UITypeSymbol UIType { get; set; } = UITypeSymbol.Null;

    /// <summary>
    /// Index number when stored in constant pool
    /// </summary>
    public virtual UniqueSymbolIndex ConstantTableIndex { get; set; } = UniqueSymbolIndex.Zero;

    /// <summary>
    /// Whether the variable is available within `on init` (for built-in variables read from external definition files)
    /// </summary>
    public virtual bool AvailableOnInit { get; set; } = true;

    /// <summary>
    /// Generated by unary operator and literal value
    /// </summary>
    public virtual bool ConstantValueWithSingleOperator { get; set; } = false;
    #endregion ~ Properties

    // TODO Implementation
}
