using System.Text;

using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;
using KSPCompiler.Shared.IO.Symbols.Yaml.UITypes.Models;

namespace KSPCompiler.Shared.IO.Symbols.Yaml.UITypes.Translators;

public sealed class SymbolToSymbolModelTranslator
    : ISymbolToSymbolModelTranslator<UITypeSymbol, UITypeSymbolModel>
{
    public UITypeSymbolModel Translate( UITypeSymbol source )
    {
        var symbol = new UITypeSymbolModel
        {
            Id                 = source.Id,
            Name               = source.Name.Value,
            BuiltIn            = source.BuiltIn,
            VariableType       = DataTypeUtility.ToString( source.DataType ),
            RequireInitializer = source.InitializerRequired,
            Description        = source.Description.Value,
            BuiltIntoVersion   = source.BuiltIntoVersion
        };

        var stringBuilder = new StringBuilder();

        foreach( var arg in source.InitializerArguments )
        {
            stringBuilder.Clear();
            DataTypeUtility.ToDataTypeString( stringBuilder, arg );

            var argument = new UITypeSymbolArgumentModel
            {
                Name        = arg.Name,
                DataType    = stringBuilder.ToString(),
                Description = arg.Description
            };

            symbol.InitializerArguments.Add( argument );
        }

        return symbol;
    }
}
