using KSPCompiler.Shared.Domain.Symbols;
using KSPCompiler.Shared.Path;
using KSPCompiler.SymbolManagement.Repository.Yaml.Variables.Models;
using KSPCompiler.SymbolManagement.Repository.Yaml.Variables.Translators;

namespace KSPCompiler.SymbolManagement.Repository.Yaml.Variables;

public class VariableSymbolRepository( FilePath repositoryPath )
    : SymbolRepository<VariableSymbol, VariableSymbolRootModel, VariableSymbolModel>(
        repositoryPath,
        new SymbolToSymbolModelTranslator(),
        new SymbolModelToSymbolTranslator()
    );
