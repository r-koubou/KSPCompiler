using KSPCompiler.Features.SymbolManagement.Infrastructures.SymbolRepository.Yaml.Commands.Models;
using KSPCompiler.Features.SymbolManagement.Infrastructures.SymbolRepository.Yaml.Commands.Translators;
using KSPCompiler.Shared.Contents;
using KSPCompiler.Shared.Domain.Symbols;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.SymbolRepository.Yaml.Commands;

public class CommandSymbolExporter( ITextContentWriter writer ) :
    SymbolExporter<
        CommandSymbol,
        CommandSymbolRootModel,
        CommandSymbolModel,
        SymbolToSymbolModelTranslator
    >( writer );
