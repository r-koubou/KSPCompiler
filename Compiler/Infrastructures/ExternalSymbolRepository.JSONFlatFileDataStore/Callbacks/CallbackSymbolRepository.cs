using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Callbacks.Models;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Callbacks.Translators;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Commons;

namespace KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Callbacks;

public class CallbackSymbolRepository : SymbolRepository<CallbackSymbol, CallbackSymbolModel>
{
    private const string CurrentVersion = "20240929";

    public CallbackSymbolRepository( FilePath repositoryPath ) : base( repositoryPath, new ToModelTranslator(), new FromModelTranslator() )
    {
        DataStore.ReplaceItem( "version", CurrentVersion, upsert: true );
    }
}
