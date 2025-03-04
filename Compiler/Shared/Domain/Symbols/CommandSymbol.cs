using System;
using System.Collections.Generic;

using KSPCompiler.Features.Compilation.Domain.Symbols.MetaData;

namespace KSPCompiler.Features.Compilation.Domain.Symbols;

public sealed class CommandSymbol : SymbolBase, ISymbolDataTypeProvider
{
    public override SymbolType Type
        => SymbolType.Command;

    public CommandArgumentSymbolList Arguments { get; } = new ();

    /// <summary>
    /// Represents the return type of the command.
    /// </summary>
    public DataTypeFlag DataType { get; set; } = DataTypeFlag.None;

    public int ArgumentCount
        => Arguments.Count;

    public CommandSymbol() {}

    public CommandSymbol( IEnumerable<CommandArgumentSymbol> args )
    {
        Arguments.AddRange( args );
    }

    public void AddArgument( CommandArgumentSymbol arg )
    {
        if( Arguments.Contains( arg ))
        {
            throw new InvalidOperationException( $"Argument {arg.Name} already exists in command {Name}" );
        }
        Arguments.Add( arg );
    }
}
