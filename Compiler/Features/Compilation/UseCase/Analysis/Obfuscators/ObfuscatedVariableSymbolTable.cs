using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Obfuscators;
using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators;

public class ObfuscatedVariableSymbolTable : ObfuscatedSymbolTable<VariableSymbol>, IObfuscatedVariableTable
{
    public ObfuscatedVariableSymbolTable( IVariableSymbolTable source, string prefix )
        : base( source, prefix ) {}

    public ObfuscatedVariableSymbolTable( IVariableSymbolTable source, string prefix, IObfuscateFormatter formatter )
        : base( source, prefix, formatter ) {}
}
