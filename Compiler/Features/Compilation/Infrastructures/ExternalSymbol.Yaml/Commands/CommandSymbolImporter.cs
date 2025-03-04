using KSPCompiler.Features.Compilation.Infrastructures.ExternalSymbol.Yaml.Commands.Models;
using KSPCompiler.Features.Compilation.Infrastructures.ExternalSymbol.Yaml.Commands.Translators;
using KSPCompiler.Shared.Contents;
using KSPCompiler.Shared.Domain.Symbols;

namespace KSPCompiler.Features.Compilation.Infrastructures.ExternalSymbol.Yaml.Commands;

public class CommandSymbolImporter( ITextContentReader writer ) :
    SymbolImporter<
        CommandSymbol,
        CommandSymbolRootModel,
        CommandSymbolModel,
        SymbolModelToSymbolTranslator
    >( writer );
