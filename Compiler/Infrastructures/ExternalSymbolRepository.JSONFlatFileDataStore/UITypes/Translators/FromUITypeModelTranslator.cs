using System.Collections.Generic;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.UITypes.Models;

namespace KSPCompiler.ExternalSymbolRepository.JSONFlatFileDataStore.UITypes.Translators;

internal class FromUITypeModelTranslator : IDataTranslator<IEnumerable<UITypeSymbolModel>, IReadOnlyCollection<UITypeSymbol>>
{
    public IReadOnlyCollection<UITypeSymbol> Translate( IEnumerable<UITypeSymbolModel> source )
    {
        var result = new List<UITypeSymbol>();

        foreach( var x in source )
        {
            var uiType = new UITypeSymbol( x.RequireInitializer )
            {
                Name             = x.Name,
                Reserved         = x.Reserved,
                Description      = x.Description,
                BuiltIntoVersion = x.BuiltIntoVersion,
                DataType         = DataTypeUtility.GuessFromTypeString( x.VariableType ),
                Modifier = ModifierFlag.UI
            };

            foreach( var arg in x.InitializerArguments )
            {
                var argument = new UIInitializerArgumentSymbol
                {
                    Name        = arg.Name,
                    Reserved    = false,
                    Description = arg.Description,
                };

                argument.DataType = DataTypeUtility.GuessFromSymbolName( argument.Name );
                uiType.AddInitializerArgument( argument );
            }

            result.Add( uiType );
        }

        return result;
    }
}
