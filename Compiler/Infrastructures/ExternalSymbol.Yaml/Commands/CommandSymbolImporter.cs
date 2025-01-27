using KSPCompiler.Commons.Contents;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbol.Yaml.Commands.Models;
using KSPCompiler.ExternalSymbol.Yaml.Commands.Translators;

namespace KSPCompiler.ExternalSymbol.Yaml.Commands;

public class CallbackSymbolImporter( ITextContentReader writer ) :
    SymbolImporter<
        CommandSymbol,
        CommandSymbolRootModel,
        CommandSymbolModel,
        SymbolModelToSymbolTranslator
    >( writer );
