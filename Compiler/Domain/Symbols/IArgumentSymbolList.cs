using System;
using System.Collections.Generic;

namespace KSPCompiler.Domain.Symbols;

public interface IArgumentSymbolList<TArgumentSymbol>
    : IList<TArgumentSymbol>, IEquatable<ArgumentSymbolList<TArgumentSymbol>>
    where TArgumentSymbol : ArgumentSymbol;
