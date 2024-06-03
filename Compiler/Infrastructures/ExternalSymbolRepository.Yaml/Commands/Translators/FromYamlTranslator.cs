using System.Collections.Generic;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.ExternalSymbolRepository.Yaml.Commands.Model;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Commands.Translators;

internal class FromYamlTranslator : IDataTranslator<RootObject, IReadOnlyCollection<CommandSymbol>>
{
    public IReadOnlyCollection<CommandSymbol> Translate( RootObject source )
    {
        var result = new List<CommandSymbol>();

        foreach( var x in source.Symbols )
        {
            var command = new CommandSymbol
            {
                Name        = x.Name,
                Reserved    = x.Reserved,
                DataType    = DataTypeUtility.Guess( x.ReturnType ),
                Description = x.Description
            };

            foreach( var arg in x.Arguments )
            {
                var argument = new CommandArgument
                {
                    Name        = arg.Name,
                    Reserved    = false,
                    Description = arg.Description,
                };

                argument.DataType = DataTypeUtility.Guess( argument.Name );
                command.AddArgument( argument );
            }

            result.Add( command );
        }

        return result;
    }
}
