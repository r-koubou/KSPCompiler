using System;
using System.Collections.Generic;

namespace KSPCompiler.Shared.Domain.Symbols;

public class CommandArgumentSymbolList
    : ArgumentSymbolList<CommandArgumentSymbol>,
      IEquatable<CommandArgumentSymbolList>
{
    public static CommandArgumentSymbolList Null { get; }
        = new NullImpl();

    public virtual bool Equals( CommandArgumentSymbolList? other )
    {
        if( other is null )
        {
            return false;
        }

        if( ReferenceEquals( this, other ) )
        {
            return true;
        }

        if( Count != other.Count )
        {
            return false;
        }

        for( var i = 0; i < Count; i++ )
        {
            var thisArg = this[ i ];
            var otherArg = other[ i ];

            if( !thisArg.Equals( otherArg ) )
            {
                return false;
            }
        }

        return true;
    }

    private class NullImpl : CommandArgumentSymbolList
    {
        public override bool Equals( CommandArgumentSymbolList? other )
        {
            return other != null && Equals( this, other );
        }

        public override IEnumerator<CommandArgumentSymbol> GetEnumerator()
            => new List<CommandArgumentSymbol>().GetEnumerator();

        public override void Add( CommandArgumentSymbol item ) {}

        public override void AddRange( IEnumerable<CommandArgumentSymbol> items ) {}

        public override void Clear() {}

        public override bool Contains( CommandArgumentSymbol item )
            => false;

        public override void CopyTo( CommandArgumentSymbol[] array, int arrayIndex ) {}

        public override bool Remove( CommandArgumentSymbol item )
            => false;
    }
}
