using System.Collections.Generic;

using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Domain.Symbols;

public sealed record CallbackSymbol( bool AllowMultipleDeclaration ) : SymbolBase
{
    public override SymbolType Type
        => SymbolType.Callback;

    public CallbackArgumentSymbolList Arguments { get; } = new();

    public int ArgumentCount
        => Arguments.Count;

    /// <summary>
    /// Is this callback allow multiple declarations
    /// </summary>
    /// <remarks>
    /// Some callbacks allows duplicated callback definitions in script.
    /// </remarks>
    public bool AllowMultipleDeclaration { get; } = AllowMultipleDeclaration;

    public CallbackSymbol( bool allowMultipleDeclaration, IEnumerable<CallbackArgumentSymbol> args ) : this( allowMultipleDeclaration )
    {
        Arguments.AddRange( args );
    }

    public CallbackSymbol DeepClone()
    {
        var newSymbol = new CallbackSymbol( AllowMultipleDeclaration )
        {
            CommentLines = CommentLines,
            DefinedPosition = DefinedPosition,
            Name = Name,
            BuiltIn = BuiltIn,
            State = State,
            Modifier = Modifier,
            TableIndex = TableIndex
        };

        foreach( var arg in Arguments )
        {
            newSymbol.Arguments.Add( arg );
        }

        return newSymbol;
    }
}
