using System;

using KSPCompiler.Commons.ValueObjects;

namespace KSPCompiler.Domain.Symbols;

public record UniqueSymbolIndex : UIntValueObject
{
    public const uint MinValue = 0;
    public const uint MaxValue = uint.MaxValue - 1;

    public static readonly UniqueSymbolIndex Null = new( uint.MaxValue );
    public static readonly UniqueSymbolIndex Zero = new( 0 );

    public UniqueSymbolIndex( uint value ) : base( value )
    {
        if( value == uint.MaxValue )
        {
            throw new ArgumentException( $"Cannot use uint.MaxValue. uint.MaxValue is reserved for {nameof( UniqueSymbolIndex )}.{nameof( Null )}" );
        }
    }
}
