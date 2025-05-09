using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;
using KSPCompiler.Shared.IO.Symbols.Yaml.UITypes.Models;

namespace KSPCompiler.Shared.IO.Symbols.Yaml.UITypes.Translators;

public sealed class SymbolModelToSymbolTranslator
    : ISymbolModelToSymbolTranslator<UITypeSymbolModel, UITypeSymbol>
{
    public UITypeSymbol Translate( UITypeSymbolModel source )
    {
        var uiType = new UITypeSymbol( source.RequireInitializer )
        {
            Id               = source.Id,
            CreatedAt        = source.CreatedAt,
            UpdatedAt        = source.UpdatedAt,
            Name             = source.Name,
            BuiltIn          = source.BuiltIn,
            Description      = source.Description,
            BuiltIntoVersion = source.BuiltIntoVersion,
            DataType         = DataTypeUtility.GuessFromTypeString( source.VariableType ),
            Modifier         = ModifierFlag.UI
        };

        foreach( var arg in source.InitializerArguments )
        {
            var argument = new UIInitializerArgumentSymbol
            {
                Name        = arg.Name,
                BuiltIn     = false,
                Description = arg.Description,
            };

            argument.DataType = DataTypeUtility.GuessFromSymbolName( argument.Name );
            uiType.AddInitializerArgument( argument );
        }

        return uiType;
    }
}
