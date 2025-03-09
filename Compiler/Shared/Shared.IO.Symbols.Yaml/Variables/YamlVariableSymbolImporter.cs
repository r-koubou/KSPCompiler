using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.IO.Abstractions.Contents;
using KSPCompiler.Shared.IO.Symbols.Yaml.Variables.Models;
using KSPCompiler.Shared.IO.Symbols.Yaml.Variables.Translators;

namespace KSPCompiler.Shared.IO.Symbols.Yaml.Variables;

public class YamlVariableSymbolImporter( ITextContentReader writer ) :
    YamlSymbolImporter<
        VariableSymbol,
        VariableSymbolRootModel,
        VariableSymbolModel,
        SymbolModelToSymbolTranslator
    >( writer );
