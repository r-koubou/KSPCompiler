using KSPCompiler.Features.SymbolManagement.Infrastructures.SymbolRepository.Yaml.Variables.Models;
using KSPCompiler.Features.SymbolManagement.Infrastructures.SymbolRepository.Yaml.Variables.Translators;
using KSPCompiler.Shared.Contents;
using KSPCompiler.Shared.Domain.Symbols;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.SymbolRepository.Yaml.Variables;

public class VariableSymbolExporter( ITextContentWriter writer ) :
    SymbolExporter<
        VariableSymbol,
        VariableSymbolRootModel,
        VariableSymbolModel,
        SymbolToSymbolModelTranslator
    >( writer );
