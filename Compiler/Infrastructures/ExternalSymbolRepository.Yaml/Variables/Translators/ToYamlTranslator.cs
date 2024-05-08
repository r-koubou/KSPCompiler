using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.Yaml.Variables.Model;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Variables.Translators;

internal class ToYamlTranslator : IDataTranslator<ISymbolTable<VariableSymbol>, RootObject>
{
    private const int Version = 1;

    public RootObject Translate( ISymbolTable<VariableSymbol> source )
    {
        var result = new RootObject
        {
            Version = Version
        };

        foreach( var x in source.Table.Values )
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
