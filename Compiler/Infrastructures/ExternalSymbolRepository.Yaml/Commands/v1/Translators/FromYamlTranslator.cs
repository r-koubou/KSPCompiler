using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.ExternalSymbol.Commons;
using KSPCompiler.ExternalSymbolRepository.Yaml.Commands.Model.v1;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Commands.Translators.v1;

internal class FromYamlTranslator : IDataTranslator<RootObject, ISymbolTable<CommandSymbol>>
{
    public ISymbolTable<CommandSymbol> Translate( RootObject source )
    {
        var result = new CommandSymbolTable();

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
                var argument = new VariableSymbol
                {
                    Name        = arg.Name,
                    Reserved    = false,
                    Description = arg.Description,
                };

                argument.DataType = DataTypeUtility.Guess( argument.Name );
                command.AddArgument( argument );
            }

            if( !result.Add( command ) )
            {
                throw new DuplicatedSymbolException( command.Name );
            }
        }

        return result;
    }
}
