using KSPCompiler.Features.Compilation.Infrastructures.ExternalSymbolRepository.Yaml.Commands.Models;
using KSPCompiler.Features.Compilation.Infrastructures.ExternalSymbolRepository.Yaml.Commands.Translators;
using KSPCompiler.Shared.Domain.Symbols;
using KSPCompiler.Shared.Path;

namespace KSPCompiler.Features.Compilation.Infrastructures.ExternalSymbolRepository.Yaml.Commands;

public class CommandSymbolRepository( FilePath repositoryPath )
    : SymbolRepository<CommandSymbol, CommandSymbolRootModel, CommandSymbolModel>(
        repositoryPath,
        new SymbolModelToSymbolTranslator()
    );
