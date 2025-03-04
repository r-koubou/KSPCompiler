using KSPCompiler.Shared.Domain.Symbols;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators.Extensions;

public static class SymbolStateExtension
{
    public static bool IsNotUsed( this SymbolState self )
        => self != SymbolState.Loaded;
}
