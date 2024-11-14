using KSPCompiler.Domain.Symbols;
using KSPCompiler.UseCases.Analysis.Obfuscators;

namespace KSPCompiler.Interactors.Analysis.Obfuscators;

public class ObfuscatedVariableSymbolTable : ObfuscatedSymbolTable<VariableSymbol>, IObfuscatedVariableTable
{
    public ObfuscatedVariableSymbolTable( IVariableSymbolTable source, string prefix )
        : base( source, prefix ) {}

    public ObfuscatedVariableSymbolTable( IVariableSymbolTable source, string prefix, IObfuscateFormatter formatter )
        : base( source, prefix, formatter ) {}
}
