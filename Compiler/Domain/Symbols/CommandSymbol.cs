using System;
using System.Collections.Generic;

using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Domain.Symbols;

public sealed class CommandSymbol : SymbolBase
{
    public override SymbolType Type
        => SymbolType.Command;

    private readonly List<VariableSymbol> arguments = new ();

    public IReadOnlyCollection<VariableSymbol> Arguments
        => arguments;

    public int ArgumentCount
        => Arguments.Count;

    public CommandSymbol() {}

    public CommandSymbol( IEnumerable<VariableSymbol> args )
    {
        arguments.AddRange( args );
    }

    public void AddArgument( VariableSymbol arg )
    {
        if( arguments.Contains( arg ))
        {
            throw new InvalidOperationException( $"Argument {arg.Name} already exists in command {Name}" );
        }
        arguments.Add( arg );
    }
}
