namespace KSPCompiler.Shared.Domain.Compilation.Symbols;

public class CommandSymbolTableNew
    : OverloadedSymbolTable<CommandSymbol, CommandArgumentSymbolList>,
      ICommandSymbolTableNew
{
    public CommandSymbolTableNew() : this( null ) {}

    public CommandSymbolTableNew(
        IOverloadedSymbolTable<CommandSymbol, CommandArgumentSymbolList>? parent = null
    ) : base( parent ) {}

    public CommandSymbolTableNew(
        UniqueSymbolIndex startUniqueIndex,
        IOverloadedSymbolTable<CommandSymbol, CommandArgumentSymbolList>? parent = null
    ) : base( startUniqueIndex, parent ) {}

    public override CommandArgumentSymbolList NoOverloadValue
        => [];
}
