using System;

namespace KSPCompiler.Shared.IO.Symbols.Yaml;

public interface ISymbolModel
{
    Guid Id { get; set; }
    string Name { get; set; }
}
