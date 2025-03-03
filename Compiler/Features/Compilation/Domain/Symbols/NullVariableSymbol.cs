using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Domain.Symbols;

/// <summary>
/// Null object for <see cref="VariableSymbol"/>
/// </summary>
public class NullVariableSymbol : VariableSymbol
{
    public static readonly VariableSymbol Instance = new NullVariableSymbol();

    /// <summary>
    /// Ctor
    /// </summary>
    private NullVariableSymbol() {}

    /// <summary>
    /// Always return <see cref="SymbolType.Unknown"/>
    /// </summary>
    public override SymbolType Type
        => SymbolType.Unknown;

    /// <summary>
    /// Always ignore set value and return 0
    /// </summary>
    public override int ArraySize
    {
        get => 0;
        set => _ = value;
    }

    /// <summary>
    /// Always ignore set value and return <see cref="UITypeSymbol.Null"/>
    /// </summary>
    public override UITypeSymbol UIType
    {
        get => UITypeSymbol.Null;
        set => _ = value;
    }

    /// <summary>
    /// Always ignore set value and return <see cref="UniqueSymbolIndex.Null"/>
    /// </summary>
    public override UniqueSymbolIndex ConstantTableIndex
    {
        get => UniqueSymbolIndex.Null;
        set => _ = value;
    }

    /// <summary>
    /// Always ignore set value and return false
    /// </summary>
    public override bool AvailableOnInit
    {
        get => false;
        set => _ = value;
    }

    /// <summary>
    /// Always ignore set value and return false
    /// </summary>
    public override bool ConstantValueWithSingleOperator
    {
        get => false;
        set => _ = value;
    }
}
