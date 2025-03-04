namespace KSPCompiler.Features.Compilation.Domain.Symbols;

public interface ICallbackSymbolTable
    : IOverloadedSymbolTable<CallbackSymbol, CallbackArgumentSymbolList>;
