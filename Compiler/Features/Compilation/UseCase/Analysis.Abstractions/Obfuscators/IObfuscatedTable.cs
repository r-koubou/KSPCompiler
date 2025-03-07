using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Obfuscators;

public interface IObfuscatedTable<in TSymbol> where TSymbol : SymbolBase
{
    bool TryGetObfuscatedByName( string original, out string result );
    string GetObfuscatedByName( string original );
}
