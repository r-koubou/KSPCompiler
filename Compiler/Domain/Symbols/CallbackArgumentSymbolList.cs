using System;
using System.Collections.Generic;

namespace KSPCompiler.Domain.Symbols;

public class CallbackArgumentSymbolList
    : ArgumentSymbolList<CallbackArgumentSymbol>,
      IEquatable<CallbackArgumentSymbolList>
{
    public static CallbackArgumentSymbolList Null { get; }
        = new NullImpl();

    public virtual bool Equals( CallbackArgumentSymbolList? other )
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

            if( thisArg.Name != otherArg.Name )
            {
                return false;
            }

            if( thisArg.DataType != otherArg.DataType )
            {
                return false;
            }

            if( thisArg.UIType != otherArg.UIType )
            {
                return false;
            }
        }

        return true;
    }

    private class NullImpl : CallbackArgumentSymbolList
    {
        public override bool Equals( CallbackArgumentSymbolList? other )
        {
            return other != null && Equals( this, other! );
        }

        public override IEnumerator<CallbackArgumentSymbol> GetEnumerator()
            => new List<CallbackArgumentSymbol>().GetEnumerator();

        public override void Add( CallbackArgumentSymbol item ) {}

        public override void AddRange( IEnumerable<CallbackArgumentSymbol> items ) {}

        public override void Clear() {}

        public override bool Contains( CallbackArgumentSymbol item )
            => false;

        public override void CopyTo( CallbackArgumentSymbol[] array, int arrayIndex )
        {
            if( array is null )
            {
                throw new ArgumentNullException( nameof( array ) );
            }

            if( arrayIndex < 0 )
            {
                throw new ArgumentOutOfRangeException( nameof( arrayIndex ) );
            }

            if( arrayIndex + Count > array.Length )
            {
                throw new ArgumentException( "Insufficient space in destination array" );
            }
        }

        public override bool Remove( CallbackArgumentSymbol item )
            => true;

        public override int Count
            => 0;

        public override bool IsReadOnly
            => false;

        public override int IndexOf( CallbackArgumentSymbol item )
            => -1;

        public override void Insert( int index, CallbackArgumentSymbol item ) {}

        public override void RemoveAt( int index ) {}

        public override CallbackArgumentSymbol this[ int index ]
        {
            get => throw new ArgumentOutOfRangeException( nameof( index ) );
            set => _ = value;
        }

        public override bool Equals( object? obj )
            => obj is CallbackArgumentSymbolList other && Equals( other );

        public override int GetHashCode()
            => HashCode.Combine( 0xDEADBEAF );

        public override string ToString()
            => "Null";
    }
}
