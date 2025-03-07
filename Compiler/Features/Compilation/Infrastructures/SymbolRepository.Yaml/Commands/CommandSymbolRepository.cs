using KSPCompiler.Features.Compilation.Infrastructures.SymbolRepository.Yaml.Commands.Models;
using KSPCompiler.Features.Compilation.Infrastructures.SymbolRepository.Yaml.Commands.Translators;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.Path;

namespace KSPCompiler.Features.Compilation.Infrastructures.SymbolRepository.Yaml.Commands;

public class CommandSymbolRepository( FilePath repositoryPath )
    : SymbolRepository<CommandSymbol, CommandSymbolRootModel, CommandSymbolModel>(
        repositoryPath,
        new SymbolModelToSymbolTranslator()
    );
