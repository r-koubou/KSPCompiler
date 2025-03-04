namespace KSPCompiler.Shared.Domain.Symbols;

public interface ICallbackSymbolTable
    : IOverloadedSymbolTable<CallbackSymbol, CallbackArgumentSymbolList>;
