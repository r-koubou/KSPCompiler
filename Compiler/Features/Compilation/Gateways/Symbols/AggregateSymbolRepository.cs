using System;

using KSPCompiler.Domain.Symbols;

namespace KSPCompiler.Gateways.Symbols;

public sealed class AggregateSymbolRepository(
    ISymbolRepository<VariableSymbol> variableSymbolRepository,
    ISymbolRepository<UITypeSymbol> uiTypeSymbolRepository,
    ISymbolRepository<CommandSymbol> commandSymbolRepository,
    ISymbolRepository<CallbackSymbol> callbackSymbolRepository ) : IDisposable
{
    public ISymbolRepository<VariableSymbol> VariableSymbolRepository { get; } = variableSymbolRepository;
    public ISymbolRepository<UITypeSymbol> UITypeSymbolRepository { get; } = uiTypeSymbolRepository;
    public ISymbolRepository<CommandSymbol> CommandSymbolRepository { get; } = commandSymbolRepository;
    public ISymbolRepository<CallbackSymbol> CallbackSymbolRepository { get; } = callbackSymbolRepository;

    public void Dispose()
    {
        DisposeImpl( VariableSymbolRepository );
        DisposeImpl( UITypeSymbolRepository );
        DisposeImpl( CommandSymbolRepository );
        DisposeImpl( CallbackSymbolRepository );
    }

    private static void DisposeImpl( IDisposable repository )
    {
        try
        {
            repository.Dispose();
        }
        catch
        {
            /* Ignore */
        }
    }
}
