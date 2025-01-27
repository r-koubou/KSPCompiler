using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.ExternalSymbolRepository.Yaml.UITypes.Models;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.UITypes.Translators;

public class SymbolToSymbolModelTranslator
    : ISymbolToSymbolModelTranslator<UITypeSymbol, UITypeSymbolModel>
{
    public UITypeSymbolModel Translate( UITypeSymbol source )
    {
        var symbol = new UITypeSymbolModel
        {
            Name               = source.Name.Value,
            BuiltIn            = source.BuiltIn,
            VariableType       = DataTypeUtility.ToString( source.DataType ),
            RequireInitializer = source.InitializerRequired,
            Description        = source.Description.Value,
            BuiltIntoVersion   = source.BuiltIntoVersion
        };

        foreach( var arg in source.InitializerArguments )
        {
            var argument = new UITypeSymbolArgumentModel
            {
                Name        = arg.Name,
                Description = arg.Description
            };

            symbol.InitializerArguments.Add( argument );
        }

        return symbol;
    }
}
