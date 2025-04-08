namespace KSPCompiler.Shared.Domain.Compilation.Symbols;

public interface ICallbackSymbolTable
    : IOverloadedSymbolTable<CallbackSymbol, CallbackArgumentSymbolList>;
