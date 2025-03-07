using KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Repository.Yaml.UITypes.Models;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Repository.Yaml.UITypes.Translators;

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
