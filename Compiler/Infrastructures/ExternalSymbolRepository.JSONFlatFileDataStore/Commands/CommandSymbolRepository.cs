using KSPCompiler.Commons.Path;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Commands.Models;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Commands.Translators;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Commons;

namespace KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Commands;

public class CommandSymbolRepository : SymbolRepository<CommandSymbol, CommandSymbolModel>
{
    private const string RepositoryIdentifier = "command";
    private const int CurrentVersion = 1;

    public CommandSymbolRepository( FilePath repositoryPath ) : base( repositoryPath, new ToCommandSymbolModelTranslator(), new FromCommandSymbolModelTranslator() )
    {
        ValidateRepositoryIdentifier( RepositoryIdentifier );
        ValidateRepositoryVersion( CurrentVersion );
    }
}
