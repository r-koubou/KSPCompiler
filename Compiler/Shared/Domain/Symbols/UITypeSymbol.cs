using System;
using System.Collections.Generic;

using KSPCompiler.Features.Compilation.Domain.Symbols.MetaData;

namespace KSPCompiler.Features.Compilation.Domain.Symbols;

public sealed class UITypeSymbol : SymbolBase, ISymbolDataTypeProvider
{
    /// <summary>
    /// Null Object
    /// </summary>
    public static readonly UITypeSymbol Null;

    /// <summary>
    /// Represents any UI type.
    /// </summary>
    public static readonly UITypeSymbol AnyUI;

    ///
    /// <inheritdoc />
    ///
    public DataTypeFlag DataType { get; set; } = DataTypeFlag.None;

    /// <summary>
    /// Static constructor
    /// </summary>
    static UITypeSymbol()
    {
        Null = new UITypeSymbol( false )
        {
            Name     = "null",
            DataType = DataTypeFlag.None,
            BuiltIn  = true
        };

        AnyUI = new UITypeSymbol( false )
        {
            Name     = "ui_*",
            DataType = DataTypeFlag.All,
            BuiltIn  = true
        };
    }

    ///
    /// <inheritdoc/>
    ///
    public override SymbolType Type
        => SymbolType.UI;

    public UIInitializerArgumentSymbolList InitializerArguments { get; } = [];

    /// <summary>
    /// True if the UI type requires an initializer.
    /// </summary>
    public bool InitializerRequired { get; }

    /// <summary>
    /// Ctor
    /// </summary>
    public UITypeSymbol( bool initializerRequired, IEnumerable<UIInitializerArgumentSymbol> initializerArguments )
    {
        InitializerRequired = initializerRequired;
        this.InitializerArguments.AddRange( initializerArguments );
        Modifier = ModifierFlag.UI;
    }

    /// <summary>
    /// Ctor
    /// </summary>
    public UITypeSymbol( bool initializerRequired ) : this( initializerRequired, new List<UIInitializerArgumentSymbol>() ) {}

    public void AddInitializerArgument( UIInitializerArgumentSymbol arg )
    {
        if( InitializerArguments.Contains( arg ))
        {
            throw new InvalidOperationException( $"Initializer argument {arg.Name} already exists in {Name}" );
        }
        InitializerArguments.Add( arg );
    }

    // TODO Implementation
}
