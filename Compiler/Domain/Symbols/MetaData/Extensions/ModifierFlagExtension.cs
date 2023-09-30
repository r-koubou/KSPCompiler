using System.Diagnostics.CodeAnalysis;

namespace KSPCompiler.Domain.Symbols.MetaData.Extensions;

[SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" )]
public static class ModifierFlagExtension
{
    public static bool IsConstant( this DataModifierFlag flag )
        => flag.HasFlag( DataModifierFlag.Const );

    public static bool AreConstant( this DataModifierFlag a, DataModifierFlag b )
        => a.IsConstant() && b.IsConstant();

    public static bool IsPolyphonic( this DataModifierFlag flag )
        => flag.HasFlag( DataModifierFlag.Polyphonic );

    public static bool ArePolyphonic( this DataModifierFlag a, DataModifierFlag b )
        => a.IsPolyphonic() && b.IsPolyphonic();

    // ReSharper disable once InconsistentNaming
    public static bool IsUI( this DataModifierFlag flag )
        => flag.HasFlag( DataModifierFlag.UI );

    // ReSharper disable once InconsistentNaming
    public static bool AreUI( this DataModifierFlag a, DataModifierFlag b )
        => a.IsUI() && b.IsUI();
}
