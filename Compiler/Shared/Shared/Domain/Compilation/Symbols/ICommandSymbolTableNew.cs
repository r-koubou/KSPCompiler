namespace KSPCompiler.Shared.Domain.Compilation.Symbols;

public interface ICommandSymbolTableNew
    : IOverloadedSymbolTable<CommandSymbol, CommandArgumentSymbolList>;
