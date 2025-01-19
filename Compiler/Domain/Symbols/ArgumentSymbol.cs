using System;
using System.Collections.Generic;
using System.Linq;

namespace KSPCompiler.Domain.Symbols;

public abstract record ArgumentSymbol : VariableSymbol
{
    public virtual IReadOnlyList<string> UITypeNames { get; }
    public virtual IReadOnlyList<string> OtherTypeNames { get; }

    protected ArgumentSymbol()
        : this( Array.Empty<string>(), Array.Empty<string>() ) {}

    protected ArgumentSymbol( IReadOnlyList<string> uiTypeNames, IReadOnlyList<string> otherTypeNames )
    {
        UITypeNames    = new List<string>( uiTypeNames );
        OtherTypeNames = new List<string>( otherTypeNames );
    }

    protected ArgumentSymbol( IReadOnlyList<string> uiTypeNames )
        : this( uiTypeNames, Array.Empty<string>() ) {}

    public virtual bool Equals( ArgumentSymbol? other )
    {
        if( ReferenceEquals( null, other ) )
        {
            return false;
        }

        if( ReferenceEquals( this, other ) )
        {
            return true;
        }

        return base.Equals( other ) &&
               UITypeNames.SequenceEqual( other.UITypeNames ) &&
               OtherTypeNames.SequenceEqual( other.OtherTypeNames );
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add( base.GetHashCode() );
        hash.Add( UITypeNames.Aggregate( 0, ( current, uiTypeName ) => HashCode.Combine( current,       uiTypeName.GetHashCode() ) ) );
        hash.Add( OtherTypeNames.Aggregate( 0, ( current, otherTypeName ) => HashCode.Combine( current, otherTypeName.GetHashCode() ) ) );

        return hash.ToHashCode();
    }
}
