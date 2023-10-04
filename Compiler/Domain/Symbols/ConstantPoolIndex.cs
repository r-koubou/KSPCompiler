using System;

using KSPCompiler.Commons.ValueObjects;

namespace KSPCompiler.Domain.Symbols;

public record ConstantPoolIndex( uint Value ) : UIntValueObject( Value )
{
    public static ConstantPoolIndex Zero => new( 0 );

}
