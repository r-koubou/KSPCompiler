using KSPCompiler.Features.Compilation.Infrastructures.ExternalSymbolRepository.Yaml.UITypes.Models;
using KSPCompiler.Features.Compilation.Infrastructures.ExternalSymbolRepository.Yaml.UITypes.Translators;
using KSPCompiler.Shared.Domain.Symbols;
using KSPCompiler.Shared.Path;

namespace KSPCompiler.Features.Compilation.Infrastructures.ExternalSymbolRepository.Yaml.UITypes;

public class UITypeSymbolRepository : SymbolRepository<UITypeSymbol, UITypeSymbolRootModel, UITypeSymbolModel>
{
    private const string RepositoryIdentifier = "ui_type";
    private const int CurrentVersion = 1;

    public UITypeSymbolRepository( FilePath repositoryPath ) : base( repositoryPath, new SymbolModelToSymbolTranslator() )
    {
    }
}
