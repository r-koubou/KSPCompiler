namespace KSPCompiler.Domain.Symbols;

public interface ICallbackSymbolTable
    : IOverloadedSymbolTable<CallbackSymbol, CallbackArgumentSymbolList>;
