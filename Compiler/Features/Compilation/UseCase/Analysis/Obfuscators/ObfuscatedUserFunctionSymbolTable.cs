using KSPCompiler.Features.Compilation.Domain.Symbols;
using KSPCompiler.Features.Compilation.UseCase.Abstractions.Obfuscators;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators;

public class ObfuscatedUserFunctionSymbolTable : ObfuscatedSymbolTable<UserFunctionSymbol>, IObfuscatedUserFunctionTable
{
    public ObfuscatedUserFunctionSymbolTable( IUserFunctionSymbolSymbolTable source, string prefix )
        : base( source, prefix ) {}

    public ObfuscatedUserFunctionSymbolTable( IUserFunctionSymbolSymbolTable source, string prefix, IObfuscateFormatter formatter )
        : base( source, prefix, formatter ) {}
}
