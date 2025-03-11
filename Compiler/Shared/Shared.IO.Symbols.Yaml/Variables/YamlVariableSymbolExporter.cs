using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Contents;
using KSPCompiler.Shared.IO.Symbols.Yaml.Variables.Models;
using KSPCompiler.Shared.IO.Symbols.Yaml.Variables.Translators;

namespace KSPCompiler.Shared.IO.Symbols.Yaml.Variables;

public class YamlVariableSymbolExporter( ITextContentWriter writer ) :
    YamlSymbolExporter<
        VariableSymbol,
        VariableSymbolRootModel,
        VariableSymbolModel,
        SymbolToSymbolModelTranslator
    >( writer );
