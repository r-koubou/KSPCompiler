using KSPCompiler.Shared.Domain.Symbols;

namespace KSPCompiler.Features.Compilation.UseCase.Abstractions.Obfuscators;

public interface IObfuscatedTable<in TSymbol> where TSymbol : SymbolBase
{
    bool TryGetObfuscatedByName( string original, out string result );
    string GetObfuscatedByName( string original );
}
