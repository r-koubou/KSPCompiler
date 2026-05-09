using System;
using System.Collections.Generic;
using System.Linq;

namespace KSPCompiler.Shared.Domain.Compilation.Symbols;

public sealed class CallbackArgumentSymbol
    : ArgumentSymbol, IEquatable<CallbackArgumentSymbol>
{
    /// <summary>
    /// This argument must be declared in the `on init` callback.
    /// </summary>
    public bool RequiredDeclareOnInit { get; }

    public CallbackArgumentSymbol( bool requiredDeclareOnInit )
    {
        RequiredDeclareOnInit = requiredDeclareOnInit;
    }

    public CallbackArgumentSymbol( bool requiredDeclareOnInit, IReadOnlyList<string> uiTypeNames, IReadOnlyList<string> otherTypeNames )
        : base( uiTypeNames, otherTypeNames )
    {
        RequiredDeclareOnInit = requiredDeclareOnInit;
    }

    public CallbackArgumentSymbol( bool requiredDeclareOnInit, IReadOnlyList<string> uiTypeNames )
        : base( uiTypeNames )
    {
        RequiredDeclareOnInit = requiredDeclareOnInit;
    }

    #region For overload callback arguments comparison
    public bool Equals( CallbackArgumentSymbol? other )
        => other != null && RequiredDeclareOnInit == other.RequiredDeclareOnInit
           && Name == other.Name
           && DataType == other.DataType
           && UITypeNames.SequenceEqual( other.UITypeNames );
    #endregion ~For overload callback arguments comparison
}
