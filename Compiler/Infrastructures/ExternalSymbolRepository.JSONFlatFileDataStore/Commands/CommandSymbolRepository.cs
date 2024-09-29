using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Commands.Models;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Commands.Translators;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Commons;

namespace KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Commands;

public class CommandSymbolRepository : SymbolRepository<CommandSymbol, CommandSymbolModel>
{
    private const string CurrentVersion = "20240929";

    public CommandSymbolRepository( FilePath repositoryPath ) : base( repositoryPath, new ToCommandSymbolModelTranslator(), new FromCommandSymbolModelTranslator() )
    {
        DataStore.ReplaceItem( "version", CurrentVersion, upsert: true );
    }
}
