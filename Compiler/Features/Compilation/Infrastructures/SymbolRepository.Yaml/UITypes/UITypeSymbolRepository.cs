using KSPCompiler.Features.Compilation.Infrastructures.SymbolRepository.Yaml.UITypes.Models;
using KSPCompiler.Features.Compilation.Infrastructures.SymbolRepository.Yaml.UITypes.Translators;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.Path;

namespace KSPCompiler.Features.Compilation.Infrastructures.SymbolRepository.Yaml.UITypes;

public class UITypeSymbolRepository : SymbolRepository<UITypeSymbol, UITypeSymbolRootModel, UITypeSymbolModel>
{
    private const string RepositoryIdentifier = "ui_type";
    private const int CurrentVersion = 1;

    public UITypeSymbolRepository( FilePath repositoryPath ) : base( repositoryPath, new SymbolModelToSymbolTranslator() )
    {
    }
}
