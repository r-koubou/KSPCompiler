using System.Collections.Generic;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.Yaml.Variables.Model;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Variables.Translators;

internal class ToYamlTranslator : IDataTranslator<IEnumerable<VariableSymbol>, RootObject>
{
    private const int Version = 1;

    public RootObject Translate( IEnumerable<VariableSymbol> source )
    {
        var result = new RootObject
        {
            Version = Version
        };

        foreach( var x in source )
        {
            var symbol = new Symbol
            {
                Name        = x.Name.Value,
                Reserved    = x.Reserved,
                Description = x.Description.Value
            };

            result.Symbols.Add( symbol );
        }

        return result;
    }
}
