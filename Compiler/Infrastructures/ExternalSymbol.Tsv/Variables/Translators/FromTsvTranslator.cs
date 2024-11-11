using System.Collections.Generic;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Domain.Symbols.MetaData;
using KSPCompiler.ExternalSymbol.Tsv.Commons;
using KSPCompiler.Infrastructures.Commons.Extensions;

using DataTypeUtility = KSPCompiler.Domain.Symbols.MetaData.DataTypeUtility;

namespace KSPCompiler.ExternalSymbol.Tsv.Variables.Translators;

internal class FromTsvTranslator : IDataTranslator<string, IReadOnlyCollection<VariableSymbol>>
{
    private enum Column
    {
        Name,
        BuiltIn,
        Description,
        BuiltIntoVersion
    }

    public IReadOnlyCollection<VariableSymbol> Translate( string source )
    {
        var result = new List<VariableSymbol>();

        TsvUtility.ParseTsv( source.SplitNewLine(), TsvUtility.RegexDefaultLineComment, values =>
            {
                TsvUtility.RemoveQuoteCharacter( values );

                var symbol = new VariableSymbol
                {
                    Name             = values[ (int)Column.Name ],
                    ArraySize        = 0,
                    BuiltIn          = TsvUtility.ParseBoolean( values[ (int)Column.BuiltIn ] ),
                    Description      = values[ (int)Column.Description ],
                    BuiltIntoVersion = values[ (int)Column.BuiltIntoVersion ]
                };

                symbol.DataType         = DataTypeUtility.GuessFromSymbolName( symbol.Name );
                symbol.Modifier = ModifierFlag.Const;

                result.Add( symbol );
            }
        );

        return result;
    }
}
