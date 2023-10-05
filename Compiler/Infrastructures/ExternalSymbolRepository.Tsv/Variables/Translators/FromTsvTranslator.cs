using System;
using System.Collections.Generic;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.ExternalSymbolRepository.Tsv.Variables.Translators;

public class FromTsvTranslator : IDataTranslator<IReadOnlyCollection<string>, ISymbolTable<VariableSymbol>>
{
    public ISymbolTable<VariableSymbol> Translate( IReadOnlyCollection<string> source )
    {
        var result = new VariableSymbolTable();

        foreach( var x in source )
        {
            var line = x.Trim();
            if( line.StartsWith( "//" ) )
            {
                continue;
            }

            var values = line.Split( '\t' );

            var symbol = new VariableSymbol
            {
                Name      = values[ 1 ],
                ArraySize = 0,
                Reserved  =  values[ 2 ] == "true"
            };

            symbol.DataType         = DataTypeUtility.FromVariableName( symbol.Name.Value );
            symbol.DataTypeModifier = DataTypeModifierFlag.Const;

            if( !result.Add( symbol ) )
            {
                throw new InvalidOperationException( $"Duplicate symbol name: {symbol.Name}" );
            }
        }

        return result;
    }
}
