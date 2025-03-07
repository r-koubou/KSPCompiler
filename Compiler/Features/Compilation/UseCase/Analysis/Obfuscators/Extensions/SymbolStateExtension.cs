using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators.Extensions;

public static class SymbolStateExtension
{
    public static bool IsNotUsed( this SymbolState self )
        => self != SymbolState.Loaded;
}
