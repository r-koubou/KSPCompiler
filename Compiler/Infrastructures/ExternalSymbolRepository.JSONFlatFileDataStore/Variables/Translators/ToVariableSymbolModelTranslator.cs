using System.Collections.Generic;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Variables.Models;

namespace KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.Variables.Translators;

internal class ToVariableSymbolModelTranslator : IDataTranslator<IEnumerable<VariableSymbol>, IReadOnlyCollection<VariableSymbolModel>>
{
    public IReadOnlyCollection<VariableSymbolModel> Translate( IEnumerable<VariableSymbol> source )
    {
        var result = new List<VariableSymbolModel>();

        foreach( var x in source )
        {
            var symbol = new VariableSymbolModel
            {
                Name        = x.Name.Value,
                Reserved    = x.Reserved,
                Description = x.Description.Value,
                BuiltIntoVersion = x.BuiltIntoVersion
            };

            result.Add( symbol );
        }

        return result;
    }
}
