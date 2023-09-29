using System;

namespace KSPCompiler.Domain.Symbols
{
    /// <summary>
    /// Representation of attributes associated with data types.
    /// </summary>
    [Flags]
    public enum DataTypeAttributeFlag
    {
        None = 0,
        Array = 1 << 0,
    }
}
