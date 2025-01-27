using KSPCompiler.Commons.Contents;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbol.Yaml.Commands.Models;
using KSPCompiler.ExternalSymbol.Yaml.Commands.Translators;

namespace KSPCompiler.ExternalSymbol.Yaml.Commands;

public class CommandSymbolExporter( ITextContentWriter writer ) :
    SymbolExporter<
        CommandSymbol,
        CommandSymbolRootModel,
        CommandSymbolModel,
        SymbolToSymbolModelTranslator
    >( writer );
