using System.Diagnostics.CodeAnalysis;

namespace KSPCompiler.Domain.Symbols.MetaData.Extensions;

[SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" )]
public static class ModifierFlagExtension
{
    public static bool IsConstant( this DataTypeModifierFlag flag )
        => flag.HasFlag( DataTypeModifierFlag.Const );

    public static bool AreConstant( this DataTypeModifierFlag a, DataTypeModifierFlag b )
        => a.IsConstant() && b.IsConstant();

    public static bool IsPolyphonic( this DataTypeModifierFlag flag )
        => flag.HasFlag( DataTypeModifierFlag.Polyphonic );

    public static bool ArePolyphonic( this DataTypeModifierFlag a, DataTypeModifierFlag b )
        => a.IsPolyphonic() && b.IsPolyphonic();

    public static bool IsUI( this DataTypeModifierFlag flag )
        => flag.HasFlag( DataTypeModifierFlag.UI );

    public static bool AreUI( this DataTypeModifierFlag a, DataTypeModifierFlag b )
        => a.IsUI() && b.IsUI();
}
