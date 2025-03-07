using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Obfuscators;
using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators;

public class ObfuscatedUserFunctionSymbolTable : ObfuscatedSymbolTable<UserFunctionSymbol>, IObfuscatedUserFunctionTable
{
    public ObfuscatedUserFunctionSymbolTable( IUserFunctionSymbolSymbolTable source, string prefix )
        : base( source, prefix ) {}

    public ObfuscatedUserFunctionSymbolTable( IUserFunctionSymbolSymbolTable source, string prefix, IObfuscateFormatter formatter )
        : base( source, prefix, formatter ) {}
}
