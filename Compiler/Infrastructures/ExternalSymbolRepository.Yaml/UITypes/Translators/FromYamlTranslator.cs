using System.Collections.Generic;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.ExternalSymbolRepository.Yaml.UITypes.Model;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.UITypes.Translators;

internal class FromYamlTranslator : IDataTranslator<RootObject, IReadOnlyCollection<UITypeSymbol>>
{
    public IReadOnlyCollection<UITypeSymbol> Translate( RootObject source )
    {
        var result = new List<UITypeSymbol>();

        foreach( var x in source.Symbols )
        {
            var uiType = new UITypeSymbol( x.RequireInitializer )
            {
                Name        = x.Name,
                Reserved    = x.Reserved,
                Description = x.Description,
                DataType    = DataTypeUtility.Guess( x.VariableType )
            };

            foreach( var arg in x.InitializerArguments )
            {
                var argument = new VariableSymbol
                {
                    Name        = arg.Name,
                    Reserved    = false,
                    Description = arg.Description,
                };

                argument.DataType = DataTypeUtility.Guess( argument.Name );
                uiType.AddInitializerArgument( argument );
            }

            result.Add( uiType );
        }

        return result;
    }
}
