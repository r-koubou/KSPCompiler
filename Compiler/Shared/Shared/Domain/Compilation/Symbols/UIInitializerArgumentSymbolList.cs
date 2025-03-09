using System;
using System.Collections.Generic;

namespace KSPCompiler.Shared.Domain.Compilation.Symbols;

public class UIInitializerArgumentSymbolList
    : ArgumentSymbolList<UIInitializerArgumentSymbol>,
      IEquatable<UIInitializerArgumentSymbolList>
{
    public static UIInitializerArgumentSymbolList Null { get; }
        = new NullImpl();

    public virtual bool Equals( UIInitializerArgumentSymbolList? other )
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

    private class NullImpl : UIInitializerArgumentSymbolList
    {
        public override bool Equals( UIInitializerArgumentSymbolList? other )
        {
            return other != null && Equals( this, other );
        }

        public override IEnumerator<UIInitializerArgumentSymbol> GetEnumerator()
            => new List<UIInitializerArgumentSymbol>().GetEnumerator();

        public override void Add( UIInitializerArgumentSymbol item ) {}

        public override void AddRange( IEnumerable<UIInitializerArgumentSymbol> items ) {}

        public override void Clear() {}

        public override bool Contains( UIInitializerArgumentSymbol item )
            => false;

        public override void CopyTo( UIInitializerArgumentSymbol[] array, int arrayIndex ) {}

        public override bool Remove( UIInitializerArgumentSymbol item )
            => false;
    }
}
