using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Domain.Symbols;

public class VariableSymbol : SymbolBase
{
    #region Properties
    public override SymbolType Type
        => SymbolType.Variable;

    /// <summary>
    /// The variable's state in semantic analysis.
    /// </summary>
    public virtual VariableState State { get; set; } = VariableState.UnInitialized;

    /// <summary>
    /// Number of elements for array type
    /// </summary>
    public virtual int ArraySize { get; set; } = -1;

    // TODO
    /// <summary>
    /// UI type when variable type is UI
    /// </summary>
    /// <seealso cref="DataTypeModifierFlag"/>
    /// <seealso cref="DataTypeModifierFlag.UI"/>
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
