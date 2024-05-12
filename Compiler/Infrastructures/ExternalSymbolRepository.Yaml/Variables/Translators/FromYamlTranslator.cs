using System.Collections.Generic;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.ExternalSymbolRepository.Yaml.Variables.Model;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Variables.Translators;

internal class FromYamlTranslator : IDataTranslator<RootObject, IReadOnlyCollection<VariableSymbol>>
{
    public IReadOnlyCollection<VariableSymbol> Translate( RootObject source )
    {
        var result = new List<VariableSymbol>();

        foreach( var x in source.Symbols )
        {
            var symbol = new VariableSymbol
            {
                Name = x.Name,
                ArraySize = 0,
                Reserved = x.Reserved,
                Description = x.Description
            };

            symbol.DataType         = DataTypeUtility.Guess( symbol.Name );
            symbol.DataTypeModifier = DataTypeModifierFlag.Const;

            result.Add( symbol );
        }

        return result;
    }
}
