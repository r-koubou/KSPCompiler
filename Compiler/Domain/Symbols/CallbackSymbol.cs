using System;
using System.Collections.Generic;

using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Domain.Symbols;

public sealed class CallbackSymbol : SymbolBase
{
    public override SymbolType Type
        => SymbolType.Callback;

    private readonly List<CallbackArgumentSymbol> arguments = new ();

    public IReadOnlyCollection<CallbackArgumentSymbol> Arguments
        => arguments;

    public int ArgumentCount
        => Arguments.Count;

    /// <summary>
    /// Is this callback allow multiple declarations
    /// </summary>
    /// <remarks>
    /// Some callbacks allows duplicated callback definitions in script.
    /// </remarks>
    public bool AllowMultipleDeclarations { get; }

    public CallbackSymbol( bool allowMultipleDeclarations )
    {
        AllowMultipleDeclarations = allowMultipleDeclarations;
    }

    public CallbackSymbol( bool allowMultipleDeclarations, IEnumerable<CallbackArgumentSymbol> args ) : this( allowMultipleDeclarations )
    {
        arguments.AddRange( args );
    }

    public void AddArgument( CallbackArgumentSymbol arg )
    {
        if( arguments.Contains( arg ))
        {
            throw new InvalidOperationException( $"Argument {arg.Name} already exists in command {Name}" );
        }
        arguments.Add( arg );
    }
}