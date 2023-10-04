using System;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.Yaml.Variables.Model.Variables;

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

            if( !result.Add( symbol.Name, symbol ) )
            {
                throw new InvalidOperationException( $"Duplicate symbol name: {symbol.Name}" );
            }
        }

        return result;
    }
}
