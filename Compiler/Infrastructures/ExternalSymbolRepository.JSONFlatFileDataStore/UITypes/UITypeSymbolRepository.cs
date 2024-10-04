using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Commons;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.UITypes.Models;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.UITypes.Translators;

namespace KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.UITypes;

public class UITypeSymbolRepository : SymbolRepository<UITypeSymbol, UITypeSymbolModel>
{
    private const string RepositoryIdentifier = "ui_type";
    private const int CurrentVersion = 1;

    public UITypeSymbolRepository( FilePath repositoryPath ) : base( repositoryPath, new ToUITypeModelTranslator(), new FromUITypeModelTranslator() )
    {
        ValidateRepositoryIdentifier( RepositoryIdentifier );
        ValidateRepositoryVersion( CurrentVersion );
    }
}
