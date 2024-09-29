using System.Collections.Generic;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Variables.Models;

namespace KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Variables.Translators;

internal class FromVariableSymbolModelTranslator : IDataTranslator<IEnumerable<VariableSymbolModel>, IReadOnlyCollection<VariableSymbol>>
{
    public IReadOnlyCollection<VariableSymbol> Translate( IEnumerable<VariableSymbolModel> source )
    {
        var result = new List<VariableSymbol>();

        foreach( var x in source )
        {
            var symbol = new VariableSymbol
            {
                Name        = x.Name,
                ArraySize   = 0,
                Reserved    = x.Reserved,
                Description = x.Description
            };

            symbol.DataType         = DataTypeUtility.Guess( symbol.Name );
            symbol.DataTypeModifier = DataTypeModifierFlag.Const;

            result.Add( symbol );
        }

        return result;
    }
}
