using KSPCompiler.Features.SymbolManagement.Infrastructures.SymbolRepository.Yaml.Variables.Models;
using KSPCompiler.Features.SymbolManagement.Infrastructures.SymbolRepository.Yaml.Variables.Translators;
using KSPCompiler.Shared.Contents;
using KSPCompiler.Shared.Domain.Symbols;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.SymbolRepository.Yaml.Variables;

public class VariableSymbolImporter( ITextContentReader writer ) :
    SymbolImporter<
        VariableSymbol,
        VariableSymbolRootModel,
        VariableSymbolModel,
        SymbolModelToSymbolTranslator
    >( writer );
