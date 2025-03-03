using KSPCompiler.Domain.Symbols;
using KSPCompiler.UseCases.Analysis.Obfuscators;

namespace KSPCompiler.Interactors.Analysis.Obfuscators;

public class ObfuscatedUserFunctionSymbolTable : ObfuscatedSymbolTable<UserFunctionSymbol>, IObfuscatedUserFunctionTable
{
    public ObfuscatedUserFunctionSymbolTable( IUserFunctionSymbolSymbolTable source, string prefix )
        : base( source, prefix ) {}

    public ObfuscatedUserFunctionSymbolTable( IUserFunctionSymbolSymbolTable source, string prefix, IObfuscateFormatter formatter )
        : base( source, prefix, formatter ) {}
}
