using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.Yaml.Commands.Models;
using KSPCompiler.ExternalSymbolRepository.Yaml.Commands.Translators;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Commands;

public class CommandSymbolRepository( FilePath repositoryPath )
    : SymbolRepository<CommandSymbol, CommandSymbolRootModel, CommandSymbolModel>(
        repositoryPath,
        new SymbolToSymbolModelTranslator(),
        new SymbolModelToSymbolTranslator()
    );
