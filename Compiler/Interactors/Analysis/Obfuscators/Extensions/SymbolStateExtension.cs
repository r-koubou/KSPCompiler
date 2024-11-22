using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Interactors.Analysis.Obfuscators.Extensions;

public static class SymbolStateExtension
{
    public static bool IsNotUsed( this SymbolState self )
        => self != SymbolState.Loaded;
}
