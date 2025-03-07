using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.Path;
using KSPCompiler.SymbolManagement.Repository.Yaml.UITypes.Models;
using KSPCompiler.SymbolManagement.Repository.Yaml.UITypes.Translators;

namespace KSPCompiler.SymbolManagement.Repository.Yaml.UITypes;

public class UITypeSymbolRepository : SymbolRepository<UITypeSymbol, UITypeSymbolRootModel, UITypeSymbolModel>
{
    private const string RepositoryIdentifier = "ui_type";
    private const int CurrentVersion = 1;

    public UITypeSymbolRepository( FilePath repositoryPath ) : base( repositoryPath, new SymbolToSymbolModelTranslator(), new SymbolModelToSymbolTranslator() )
    {
    }
}
