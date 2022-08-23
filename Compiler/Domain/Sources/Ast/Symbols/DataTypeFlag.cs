using System;

namespace KSPCompiler.Domain.Ast.Symbols
{
    /// <summary>
    /// Representation of attributes associated with data types.
    /// </summary>
    [Flags]
    public enum DataTypeFlag
    {
        None = 0,
        Array = 1 << 0,
    }
}
