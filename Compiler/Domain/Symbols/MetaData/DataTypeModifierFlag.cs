using System;

namespace KSPCompiler.Domain.Symbols.MetaData
{
    /// <summary>
    /// Symbol modifier definition.
    /// </summary>
    [Flags]
    public enum DataTypeModifierFlag
    {
        None = 0,
        Const = 1 << 0,
        Polyphonic = 1 << 1,
        // ReSharper disable once InconsistentNaming
        UI = 1 << 2,
    }
}
