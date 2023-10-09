using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;

namespace KSPCompiler.ExternalSymbolRepository.Tsv.Variables.Translators;

public class FromTsvTranslator : IDataTranslator<IReadOnlyCollection<string>, ISymbolTable<VariableSymbol>>
{
    private enum Column
    {
        Name,
        Reserved,
        Description
    }

    private static readonly Regex LineComment = new( @"^#.*" );

    public ISymbolTable<VariableSymbol> Translate( IReadOnlyCollection<string> source )
    {
        var result = new VariableSymbolTable();

        foreach( var x in source )
        {
            if( LineComment.IsMatch( x ) )
            {
                continue;
            }

            var values = x.Split( '\t' );

            var symbol = new VariableSymbol
            {
                Name        = values[ (int)Column.Name ],
                ArraySize   = 0,
                Reserved    = values[ (int)Column.Reserved ] == "true",
                Description = values[ (int)Column.Description ]
            };

            symbol.DataType         = DataTypeUtility.FromVariableName( symbol.Name );
            symbol.DataTypeModifier = DataTypeModifierFlag.Const;

            if( !result.Add( symbol ) )
            {
                throw new InvalidOperationException( $"Duplicate symbol name: {symbol.Name}" );
            }
        }

        return result;
    }
}
