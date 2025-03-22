namespace KSPCompiler.Shared.Domain.Compilation.Symbols;

public class CommandSymbolTable
    : OverloadedSymbolTable<CommandSymbol, CommandArgumentSymbolList>,
      ICommandSymbolTable
{
    public CommandSymbolTable() : this( null ) {}

    public CommandSymbolTable(
        IOverloadedSymbolTable<CommandSymbol, CommandArgumentSymbolList>? parent = null
    ) : base( parent ) {}

    public CommandSymbolTable(
        UniqueSymbolIndex startUniqueIndex,
        IOverloadedSymbolTable<CommandSymbol, CommandArgumentSymbolList>? parent = null
    ) : base( startUniqueIndex, parent ) {}

    public override CommandArgumentSymbolList NoOverloadValue
        => [];
}
