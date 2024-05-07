using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.ExternalSymbolRepository.Yaml.Commands.v1.Model;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Commands.v1.Translators;

internal class ToYamlTranslator : IDataTranslator<ISymbolTable<CommandSymbol>, RootObject>
{
    public RootObject Translate( ISymbolTable<CommandSymbol> source )
    {
        var result = new RootObject();

        foreach( var x in source.Table.Values )
        {
            var symbol = new Symbol
            {
                Name        = x.Name.Value,
                Reserved    = x.Reserved,
                ReturnType  = DataTypeUtility.ToString( x.DataType ),
                Description = x.Description.Value
            };

            foreach( var arg in x.Arguments )
            {
                var argument = new Argument
                {
                    Name        = arg.Name,
                    Description = arg.Description
                };

                symbol.Arguments.Add( argument );
            }

            result.Symbols.Add( symbol );
        }

        return result;
    }
}
