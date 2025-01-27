using System.Collections.Generic;

using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.ExternalSymbol.Yaml.Commands.Models;

namespace KSPCompiler.ExternalSymbol.Yaml.Commands.Translators;

public class SymbolModelToSymbolTranslator
    : ISymbolModelToSymbolTranslator<CommandSymbolModel, CommandSymbol>
{
    public CommandSymbol Translate( CommandSymbolModel source )
    {
        var symbol = new CommandSymbol
        {
            Name             = source.Name,
            BuiltIn          = source.BuiltIn,
            DataType         = DataTypeUtility.GuessFromTypeString( source.ReturnType ),
            Description      = source.Description,
            BuiltIntoVersion = source.BuiltIntoVersion
        };

        foreach( var arg in source.Arguments )
        {
            var uiType = new List<string>();
            var otherType = new List<string>();

            DataTypeUtility.GuessFromTypeString( arg.DataType, out var dataType, ref uiType, ref otherType );

            var argument = new CommandArgumentSymbol
            {
                Name        = arg.Name,
                DataType    = dataType,
                BuiltIn     = false,
                Description = arg.Description
            };

            symbol.AddArgument( argument );
        }

        return symbol;
    }
}
