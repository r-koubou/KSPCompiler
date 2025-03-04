using KSPCompiler.Shared.Domain.Symbols.MetaData;

namespace KSPCompiler.Shared.Domain.Symbols;

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
