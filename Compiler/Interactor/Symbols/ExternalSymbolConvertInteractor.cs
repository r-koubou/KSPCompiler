using KSPCompiler.Domain.Symbols;
using KSPCompiler.UseCases.Symbols;

namespace KSPCompiler.Interactor.Symbols;

public class ExternalSymbolConvertInteractor : IExternalSymbolConvertUseCase
{
    public void Convert<TSymbol>( ISymbolRepository<TSymbol> source, ISymbolRepository<TSymbol> target ) where TSymbol : SymbolBase
    {
        var table = source.LoadSymbolTable();
        target.StoreSymbolTable( table );
    }
}
