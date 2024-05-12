using System.Collections.Generic;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.UseCases.Symbols;

public interface IVariableSymbolStoreUseCase : IUseCase<IEnumerable<VariableSymbol>, Unit>
{}
