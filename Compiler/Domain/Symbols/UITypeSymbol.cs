using System;
using System.Collections.Generic;

using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.Domain.Symbols;

public sealed class UITypeSymbol : SymbolBase
{
    /// <summary>
    /// Null Object
    /// </summary>
    public static readonly UITypeSymbol Null;

    /// <summary>
    /// Represents any UI type.
    /// </summary>
    public static readonly UITypeSymbol AnyUI;

    /// <summary>
    /// Static constructor
    /// </summary>
    static UITypeSymbol()
    {
        Null = new UITypeSymbol( false )
        {
            Name             = "null",
            DataType         = DataTypeFlag.None,
            Reserved = true
        };

        AnyUI = new UITypeSymbol( false )
        {
            Name             = "ui_*",
            DataType         = DataTypeFlag.MultipleType,
            Reserved = true
        };
    }

    ///
    /// <inheritdoc/>
    ///
    public override SymbolType Type
        => SymbolType.UI;

    private readonly List<VariableSymbol> initializerArguments = new ();

    public IReadOnlyList<VariableSymbol> InitializerArguments
        => initializerArguments;

    /// <summary>
    /// True if the UI type requires an initializer.
    /// </summary>
    public bool InitializerRequired { get; }

    /// <summary>
    /// Ctor
    /// </summary>
    public UITypeSymbol( bool initializerRequired, IEnumerable<VariableSymbol> initializerArguments )
    {
        InitializerRequired = initializerRequired;
        this.initializerArguments.AddRange( initializerArguments );
        DataTypeModifier = DataTypeModifierFlag.UI;
    }

    /// <summary>
    /// Ctor
    /// </summary>
    public UITypeSymbol( bool initializerRequired ) : this( initializerRequired, new List<VariableSymbol>() ) {}

    public void AddInitializerArgument( VariableSymbol arg )
    {
        if( initializerArguments.Contains( arg ))
        {
            throw new InvalidOperationException( $"Initializer argument {arg.Name} already exists in {Name}" );
        }
        initializerArguments.Add( arg );
    }

    // TODO Implementation
}
