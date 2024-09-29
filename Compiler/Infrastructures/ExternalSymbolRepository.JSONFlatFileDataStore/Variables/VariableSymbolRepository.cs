using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Commons;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Variables.Models;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Variables.Translators;

namespace KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Variables;

public class VariableSymbolRepository : SymbolRepository<VariableSymbol, VariableSymbolModel>
{
    private const string CurrentVersion = "20240929";

    public VariableSymbolRepository( FilePath repositoryPath ) : base( repositoryPath, new ToVariableSymbolModelTranslator(), new FromVariableSymbolModelTranslator() )
    {
        DataStore.ReplaceItem( "version", CurrentVersion, upsert: true );
    }
}
