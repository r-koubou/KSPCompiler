using System.Collections.Generic;

namespace KSPCompiler.Domain.Symbols;

public interface IArgumentSymbolList<TArgumentSymbol>
    : IList<TArgumentSymbol>
    where TArgumentSymbol : ArgumentSymbol;
