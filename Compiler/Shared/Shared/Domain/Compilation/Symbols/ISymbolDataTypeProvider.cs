using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;

namespace KSPCompiler.Shared.Domain.Compilation.Symbols;

/// <summary>
/// Represents a data type of symbol if it owns a data type.
/// </summary>
public interface ISymbolDataTypeProvider
{
    /// <summary>
    /// Data type of the symbol.
    /// </summary>
    DataTypeFlag DataType { get; set; }
}
