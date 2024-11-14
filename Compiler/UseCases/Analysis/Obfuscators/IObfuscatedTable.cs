using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.UseCases.Analysis.Obfuscators;

public interface IObfuscatedTable<in TSymbol> where TSymbol : SymbolBase
{
    bool TryGetObfuscatedByName( string original, out string result );
    string GetObfuscatedByName( string original );
}

public interface IObfuscatedVariableTable : IObfuscatedTable<VariableSymbol> {}
public interface IObfuscatedUserFunctionTable : IObfuscatedTable<UserFunctionSymbol> {}
