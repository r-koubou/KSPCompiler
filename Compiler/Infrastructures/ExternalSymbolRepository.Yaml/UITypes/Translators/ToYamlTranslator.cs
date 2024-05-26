using System.Collections.Generic;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.ExternalSymbolRepository.Yaml.UITypes.Model;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.UITypes.Translators;

internal class ToYamlTranslator : IDataTranslator<IEnumerable<UITypeSymbol>, RootObject>
{
    public RootObject Translate( IEnumerable<UITypeSymbol> source )
    {
        var result = new RootObject();

        foreach( var x in source )
        {
            var symbol = new Symbol
            {
                Name        = x.Name.Value,
                Reserved    = x.Reserved,
                VariableType  = DataTypeUtility.ToString( x.DataType ),
                RequireInitializer = x.InitializerRequired,
                Description = x.Description.Value
            };

            foreach( var arg in x.InitializerArguments )
            {
                var argument = new InitializerArgument
                {
                    Name        = arg.Name,
                    Description = arg.Description
                };

                symbol.InitializerArguments.Add( argument );
            }

            result.Symbols.Add( symbol );
        }

        return result;
    }
}
