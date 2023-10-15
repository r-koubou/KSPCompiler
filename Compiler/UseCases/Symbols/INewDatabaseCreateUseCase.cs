using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.UseCases.Symbols;

public interface INewDatabaseCreateUseCase
{
    void Create( IVariableSymbolRepository repository );
}
