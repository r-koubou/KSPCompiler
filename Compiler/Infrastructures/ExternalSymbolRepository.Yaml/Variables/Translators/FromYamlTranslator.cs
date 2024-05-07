using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.ExternalSymbol.Commons;
using KSPCompiler.ExternalSymbolRepository.Yaml.Variables.Model;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Variables.Translators;

internal class FromYamlTranslator : IDataTranslator<RootObject, ISymbolTable<VariableSymbol>>
{
    public ISymbolTable<VariableSymbol> Translate( RootObject source )
    {
        var result = new VariableSymbolTable();

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

            if( !result.Add( symbol ) )
            {
                throw new DuplicatedSymbolException( symbol.Name );
            }
        }

        return result;
    }
}
