using KSPCompiler.Commons.Contents;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbol.Yaml.Variables.Models;
using KSPCompiler.ExternalSymbol.Yaml.Variables.Translators;

namespace KSPCompiler.ExternalSymbol.Yaml.Variables;

public class VariableSymbolExporter( ITextContentWriter writer ) :
    SymbolExporter<
        VariableSymbol,
        VariableSymbolRootModel,
        VariableSymbolModel,
        SymbolToSymbolModelTranslator
    >( writer );
