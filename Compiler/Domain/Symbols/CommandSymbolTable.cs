namespace KSPCompiler.Domain.Symbols;

public class CommandSymbolTable : SymbolTable<CommandSymbol>
{
    public override void OnWillAdd( CommandSymbol symbol ) {}
}
