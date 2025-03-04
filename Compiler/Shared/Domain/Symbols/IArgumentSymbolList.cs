using System.Collections.Generic;

namespace KSPCompiler.Shared.Domain.Symbols;

public interface IArgumentSymbolList<TArgumentSymbol>
    : IList<TArgumentSymbol>
    where TArgumentSymbol : ArgumentSymbol;
