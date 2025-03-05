using KSPCompiler.Features.Compilation.Infrastructures.SymbolRepository.Yaml.Variables.Models;
using KSPCompiler.Features.Compilation.Infrastructures.SymbolRepository.Yaml.Variables.Translators;
using KSPCompiler.Shared.Domain.Symbols;
using KSPCompiler.Shared.Path;

namespace KSPCompiler.Features.Compilation.Infrastructures.SymbolRepository.Yaml.Variables;

public class VariableSymbolRepository( FilePath repositoryPath )
    : SymbolRepository<VariableSymbol, VariableSymbolRootModel, VariableSymbolModel>(
        repositoryPath,
        new SymbolModelToSymbolTranslator()
    );
