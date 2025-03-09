using System.Diagnostics.CodeAnalysis;

namespace KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData.Extensions;

[SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" )]
public static class ModifierFlagExtension
{
    public static bool IsConstant( this ModifierFlag flag )
        => flag.HasFlag( ModifierFlag.Const );

    public static bool AreConstant( this ModifierFlag a, ModifierFlag b )
        => a.IsConstant() && b.IsConstant();

    public static bool IsPolyphonic( this ModifierFlag flag )
        => flag.HasFlag( ModifierFlag.Polyphonic );

    public static bool ArePolyphonic( this ModifierFlag a, ModifierFlag b )
        => a.IsPolyphonic() && b.IsPolyphonic();

    public static bool IsUI( this ModifierFlag flag )
        => flag.HasFlag( ModifierFlag.UI );

    public static bool AreUI( this ModifierFlag a, ModifierFlag b )
        => a.IsUI() && b.IsUI();
}
