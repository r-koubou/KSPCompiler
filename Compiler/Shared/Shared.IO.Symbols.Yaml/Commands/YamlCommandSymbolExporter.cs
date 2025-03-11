using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Contents;
using KSPCompiler.Shared.IO.Symbols.Yaml.Commands.Models;
using KSPCompiler.Shared.IO.Symbols.Yaml.Commands.Translators;

namespace KSPCompiler.Shared.IO.Symbols.Yaml.Commands;

public class YamlCommandSymbolExporter( ITextContentWriter writer ) :
    YamlSymbolExporter<
        CommandSymbol,
        CommandSymbolRootModel,
        CommandSymbolModel,
        SymbolToSymbolModelTranslator
    >( writer );
