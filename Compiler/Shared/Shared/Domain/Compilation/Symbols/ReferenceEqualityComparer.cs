using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace KSPCompiler.Shared.Domain.Compilation.Symbols;

internal sealed class ReferenceEqualityComparer<T> : IEqualityComparer<T>
    where T : class
{
    public static readonly ReferenceEqualityComparer<T> Instance = new();

    private ReferenceEqualityComparer() {}

    public bool Equals( T? x, T? y )
        => ReferenceEquals( x, y );

    public int GetHashCode( T obj )
        => RuntimeHelpers.GetHashCode( obj );
}
