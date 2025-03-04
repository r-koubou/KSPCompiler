using KSPCompiler.Shared.Domain.Symbols;

namespace KSPCompiler.Features.Compilation.UseCase.Abstractions.Obfuscators;

public interface IObfuscateFormatter
{
    string Format( string original, string prefix, UniqueSymbolIndex index );
}
