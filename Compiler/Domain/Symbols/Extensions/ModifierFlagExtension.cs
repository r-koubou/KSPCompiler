namespace KSPCompiler.Domain.Symbols.Extensions;

public static class ModifierFlagExtension
{
    public static bool IsConstant( this ModifierFlag modifier ) => ( modifier & ModifierFlag.Const ) != 0;
    public static bool AreConstant( this ModifierFlag a, ModifierFlag b ) => a.IsConstant() && b.IsConstant();

    public static bool IsPolyphonic( this ModifierFlag modifier ) => ( modifier & ModifierFlag.Polyphonic ) != 0;
    public static bool ArePolyphonic( this ModifierFlag a, ModifierFlag b ) => a.IsPolyphonic() && b.IsPolyphonic();

    // ReSharper disable once InconsistentNaming
    public static bool IsUI( this ModifierFlag modifier ) => ( modifier & ModifierFlag.UI ) != 0;
    // ReSharper disable once InconsistentNaming
    public static bool AreUI( this ModifierFlag a, ModifierFlag b ) => a.IsUI() && b.IsUI();
}