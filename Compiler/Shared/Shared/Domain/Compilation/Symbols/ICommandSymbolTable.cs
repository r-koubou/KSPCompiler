namespace KSPCompiler.Shared.Domain.Compilation.Symbols;

public interface ICommandSymbolTable
    : IOverloadedSymbolTable<CommandSymbol, CommandArgumentSymbolList>;
