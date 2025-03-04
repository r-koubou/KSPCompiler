using System.Collections.Generic;

namespace KSPCompiler.Features.Compilation.Domain.Symbols;

public interface IArgumentSymbolList<TArgumentSymbol>
    : IList<TArgumentSymbol>
    where TArgumentSymbol : ArgumentSymbol;
