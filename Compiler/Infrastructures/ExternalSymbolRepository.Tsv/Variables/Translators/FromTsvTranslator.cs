using System.Collections.Generic;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.ExternalSymbolRepository.Tsv.Commons;
using KSPCompiler.Infrastructures.Commons.Extensions;

using DataTypeUtility = KSPCompiler.Domain.Symbols.MetaData.DataTypeUtility;

namespace KSPCompiler.ExternalSymbolRepository.Tsv.Variables.Translators;

internal class FromTsvTranslator : IDataTranslator<string, IReadOnlyCollection<VariableSymbol>>
{
    private enum Column
    {
        Name,
        Reserved,
        Description
    }

    public IReadOnlyCollection<VariableSymbol> Translate( string source )
    {
        var result = new List<VariableSymbol>();

        TsvUtility.ParseTsv( source.SplitNewLine(), TsvUtility.RegexDefaultLineComment, values =>
            {
                TsvUtility.RemoveQuoteCharacter( values );

                var symbol = new VariableSymbol
                {
                    Name        = values[ (int)Column.Name ],
                    ArraySize   = 0,
                    Reserved    = TsvUtility.ParseBoolean( values[ (int)Column.Reserved ] ),
                    Description = values[ (int)Column.Description ]
                };

                symbol.DataType         = DataTypeUtility.Guess( symbol.Name );
                symbol.DataTypeModifier = DataTypeModifierFlag.Const;

                result.Add( symbol );
            }
        );

        return result;
    }
}
