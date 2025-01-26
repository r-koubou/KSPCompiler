using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.Yaml.Variables.Models;
using KSPCompiler.ExternalSymbolRepository.Yaml.Variables.Translators;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Variables;

public class VariableSymbolRepository( FilePath repositoryPath )
    : SymbolRepository<VariableSymbol, VariableSymbolRootModel, VariableSymbolModel>(
        repositoryPath,
        new SymbolToSymbolModelTranslator(),
        new SymbolModelToSymbolTranslator()
    );
