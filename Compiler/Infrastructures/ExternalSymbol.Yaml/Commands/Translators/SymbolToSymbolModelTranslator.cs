using System.Text;

using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.ExternalSymbol.Yaml.Commands.Models;

namespace KSPCompiler.ExternalSymbol.Yaml.Commands.Translators;

public class SymbolToSymbolModelTranslator
    : ISymbolToSymbolModelTranslator<CommandSymbol, CommandSymbolModel>
{
    public CommandSymbolModel Translate( CommandSymbol source )
    {
        var model = new CommandSymbolModel
        {
            Name             = source.Name.Value,
            BuiltIn          = source.BuiltIn,
            ReturnType       = DataTypeUtility.ToString( source.DataType ),
            Description      = source.Description.Value,
            BuiltIntoVersion = source.BuiltIntoVersion
        };

        var stringBuilder = new StringBuilder();

        foreach( var arg in source.Arguments )
        {
            stringBuilder.Clear();
            DataTypeUtility.ToDataTypeString( stringBuilder, arg );

            var argument = new CommandArgumentModel
            {
                Name        = arg.Name,
                DataType    = stringBuilder.ToString(),
                Description = arg.Description
            };

            model.Arguments.Add( argument );
        }

        return model;
    }
}
