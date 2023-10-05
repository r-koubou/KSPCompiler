using System;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.ExternalSymbolRepository.Yaml.Variables.Model;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Variables.Translators;

public class FromYamlTranslator : IDataTranslator<RootObject, ISymbolTable<VariableSymbol>>
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
                Reserved = x.Reserved
            };

            symbol.DataType         = DataTypeUtility.FromVariableName( symbol.Name.Value );
            symbol.DataTypeModifier = DataTypeModifierFlag.None;

            if( !result.Add( symbol ) )
            {
                throw new InvalidOperationException( $"Duplicate symbol name: {symbol.Name}" );
            }
        }

        return result;
    }
}
