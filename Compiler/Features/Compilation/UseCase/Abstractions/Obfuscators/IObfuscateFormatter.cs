using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.UseCases.Analysis.Obfuscators;

public interface IObfuscateFormatter
{
    string Format( string original, string prefix, UniqueSymbolIndex index );
}
