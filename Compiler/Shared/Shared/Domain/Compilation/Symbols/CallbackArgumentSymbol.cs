using System;
using System.Linq;

namespace KSPCompiler.Shared.Domain.Compilation.Symbols;

public sealed record CallbackArgumentSymbol( bool RequiredDeclareOnInit )
    : ArgumentSymbol, IEquatable<CallbackArgumentSymbol>
{
    /// <summary>
    /// This argument must be declared in the `on init` callback.
    /// </summary>
    public bool RequiredDeclareOnInit { get; } = RequiredDeclareOnInit;

    #region For overload callback arguments comparison
    public bool Equals( CallbackArgumentSymbol other )
        => RequiredDeclareOnInit == other.RequiredDeclareOnInit
           && Name == other.Name
           && DataType == other.DataType
           && UITypeNames.SequenceEqual( other.UITypeNames );

    public override int GetHashCode()
        => HashCode.Combine( RequiredDeclareOnInit , Name, DataType, UITypeNames );
    #endregion ~For overload callback arguments comparison
}
