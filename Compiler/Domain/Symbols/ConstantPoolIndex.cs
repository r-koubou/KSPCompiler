using System;

using KSPCompiler.Commons.ValueObjects;

namespace KSPCompiler.Domain.Symbols;

public record ConstantPoolIndex : UIntValueObject
{
    public const uint MinValue = 0;
    public const uint MaxValue = uint.MaxValue - 1;

    public static readonly ConstantPoolIndex Null = new( uint.MaxValue );
    public static readonly ConstantPoolIndex Zero = new( 0 );

    public ConstantPoolIndex( uint value ) : base( value )
    {
        if( value == uint.MaxValue )
        {
            throw new ArgumentException( $"Cannot use uint.MaxValue. uint.MaxValue is reserved for {nameof( ConstantPoolIndex )}.{nameof( Null )}" );
        }
    }
}
