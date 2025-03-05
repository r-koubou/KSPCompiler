using KSPCompiler.Shared.Domain.Symbols;
using KSPCompiler.Shared.Path;
using KSPCompiler.SymbolManagement.Repository.Yaml.Commands.Models;
using KSPCompiler.SymbolManagement.Repository.Yaml.Commands.Translators;

namespace KSPCompiler.SymbolManagement.Repository.Yaml.Commands;

public class CommandSymbolRepository( FilePath repositoryPath )
    : SymbolRepository<CommandSymbol, CommandSymbolRootModel, CommandSymbolModel>(
        repositoryPath,
        new SymbolToSymbolModelTranslator(),
        new SymbolModelToSymbolTranslator()
    );
