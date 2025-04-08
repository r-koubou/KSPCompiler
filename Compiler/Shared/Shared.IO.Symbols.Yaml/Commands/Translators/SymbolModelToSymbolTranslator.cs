using System.Collections.Generic;

using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;
using KSPCompiler.Shared.IO.Symbols.Yaml.Commands.Models;

namespace KSPCompiler.Shared.IO.Symbols.Yaml.Commands.Translators;

public class SymbolModelToSymbolTranslator
    : ISymbolModelToSymbolTranslator<CommandSymbolModel, CommandSymbol>
{
    public CommandSymbol Translate( CommandSymbolModel source )
    {
        var command = new CommandSymbol
        {
            Id               = source.Id,
            CreatedAt        = source.CreatedAt,
            UpdatedAt        = source.UpdatedAt,
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

            var argument = new CommandArgumentSymbol( uiType, otherType )
            {
                Name        = arg.Name,
                DataType    = dataType,
                BuiltIn     = false,
                Description = arg.Description,
            };

            command.AddArgument( argument );
        }

        return command;
    }
}
