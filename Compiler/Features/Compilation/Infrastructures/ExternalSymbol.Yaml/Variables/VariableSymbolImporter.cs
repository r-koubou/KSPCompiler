using KSPCompiler.Features.Compilation.Infrastructures.ExternalSymbol.Yaml.Variables.Models;
using KSPCompiler.Features.Compilation.Infrastructures.ExternalSymbol.Yaml.Variables.Translators;
using KSPCompiler.Shared.Contents;
using KSPCompiler.Shared.Domain.Symbols;

namespace KSPCompiler.Features.Compilation.Infrastructures.ExternalSymbol.Yaml.Variables;

public class VariableSymbolImporter( ITextContentReader writer ) :
    SymbolImporter<
        VariableSymbol,
        VariableSymbolRootModel,
        VariableSymbolModel,
        SymbolModelToSymbolTranslator
    >( writer );
