using System;

namespace KSPCompiler.Domain.Ast.Constants
{
    /// <summary>
    /// Symbol modifier definition.
    /// </summary>
    [Flags]
    public enum ModifierFlag
    {
        None = 0,
        Const = 1 << 0,
        Polyphonic = 1 << 1,
        UI = 1 << 2,
    }

    public static class ModifierFlagExtension
    {
        public static bool IsConstant( this ModifierFlag modifier ) => ( modifier & ModifierFlag.Const ) != 0;
        public static bool AreConstant( this ModifierFlag a, ModifierFlag b ) => a.IsConstant() && b.IsConstant();

        public static bool IsPolyphonic( this ModifierFlag modifier ) => ( modifier & ModifierFlag.Polyphonic ) != 0;
        public static bool ArePolyphonic( this ModifierFlag a, ModifierFlag b ) => a.IsPolyphonic() && b.IsPolyphonic();

        public static bool IsUI( this ModifierFlag modifier ) => ( modifier & ModifierFlag.UI ) != 0;
        public static bool AreUI( this ModifierFlag a, ModifierFlag b ) => a.IsUI() && b.IsUI();
    }
}
