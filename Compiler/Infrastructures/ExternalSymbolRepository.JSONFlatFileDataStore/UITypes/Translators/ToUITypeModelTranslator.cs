using System.Collections.Generic;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.UITypes.Models;

namespace KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.UITypes.Translators;

internal class ToUITypeModelTranslator : IDataTranslator<IEnumerable<UITypeSymbol>, IReadOnlyCollection<UITypeSymbolModel>>
{
    public IReadOnlyCollection<UITypeSymbolModel> Translate( IEnumerable<UITypeSymbol> source )
    {
        var result = new List<UITypeSymbolModel>();

        foreach( var x in source )
        {
            var symbol = new UITypeSymbolModel
            {
                Name               = x.Name.Value,
                Reserved           = x.Reserved,
                VariableType       = DataTypeUtility.ToString( x.DataType ),
                RequireInitializer = x.InitializerRequired,
                Description        = x.Description.Value,
                BuiltIntoVersion   = x.BuiltIntoVersion
            };

            foreach( var arg in x.InitializerArguments )
            {
                var argument = new UITypeSymbolArgumentModel
                {
                    Name        = arg.Name,
                    Description = arg.Description
                };

                symbol.InitializerArguments.Add( argument );
            }

            result.Add( symbol );
        }

        return result;
    }
}
