using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Callbacks.Models;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Callbacks.Translators;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Commons;

namespace KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Callbacks;

public class CallbackSymbolRepository : SymbolRepository<CallbackSymbol, CallbackSymbolModel>
{
    private const string RepositoryIdentifier = "callback";
    private const int CurrentVersion = 1;

    public CallbackSymbolRepository( FilePath repositoryPath ) : base( repositoryPath, new ToCallbackSymbolModelTranslator(), new FromCallbackSymbolModelTranslator() )
    {
        ValidateRepositoryIdentifier( RepositoryIdentifier );
        ValidateRepositoryVersion( CurrentVersion );
    }
}
