using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Obfuscators;

public interface IObfuscateFormatter
{
    string Format( string original, string prefix, UniqueSymbolIndex index );
}
