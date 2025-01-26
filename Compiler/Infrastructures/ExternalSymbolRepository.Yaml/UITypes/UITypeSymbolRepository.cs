using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.Yaml.UITypes.Models;
using KSPCompiler.ExternalSymbolRepository.Yaml.UITypes.Translators;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.UITypes;

public class UITypeSymbolRepository : SymbolRepository<UITypeSymbol, UITypeSymbolRootModel, UITypeSymbolModel>
{
    private const string RepositoryIdentifier = "ui_type";
    private const int CurrentVersion = 1;

    public UITypeSymbolRepository( FilePath repositoryPath ) : base( repositoryPath, new ToUITypeModelTranslator(), new FromUITypeModelTranslator() )
    {
    }
}
