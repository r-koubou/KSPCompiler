using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.ExternalSymbol.Yaml.UITypes.Models;

namespace KSPCompiler.ExternalSymbol.Yaml.UITypes.Translators;

public class SymbolModelToSymbolTranslator
    : ISymbolModelToSymbolTranslator<UITypeSymbolModel, UITypeSymbol>
{
    public UITypeSymbol Translate( UITypeSymbolModel source )
    {
        var uiType = new UITypeSymbol( source.RequireInitializer )
        {
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
