using KSPCompiler.Commons.Contents;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbol.Yaml.Variables.Models;
using KSPCompiler.ExternalSymbol.Yaml.Variables.Translators;

namespace KSPCompiler.ExternalSymbol.Yaml.Variables;

public class VariableSymbolImporter( ITextContentReader writer ) :
    SymbolImporter<
        VariableSymbol,
        VariableSymbolRootModel,
        VariableSymbolModel,
        SymbolModelToSymbolTranslator
    >( writer );
