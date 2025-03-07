using System;
using System.Text;

using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;
using KSPCompiler.SymbolManagement.Repository.Yaml.Commands.Models;

namespace KSPCompiler.SymbolManagement.Repository.Yaml.Commands.Translators;

public class SymbolToSymbolModelTranslator
    : ISymbolToSymbolModelTranslator<CommandSymbol, CommandSymbolModel>
{
    public CommandSymbolModel Translate( CommandSymbol source )
    {
        var model = new CommandSymbolModel
        {
            Id               = Guid.NewGuid(),
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
