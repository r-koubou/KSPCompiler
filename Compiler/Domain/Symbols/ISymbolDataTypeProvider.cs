using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Domain.Symbols;

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
