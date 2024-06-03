using System.Collections.Generic;
using System.Text.RegularExpressions;

using KSPCompiler.Commons;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.ExternalSymbolRepository.Tsv.Commons;
using KSPCompiler.Infrastructures.Commons.Extensions;

using DataTypeUtility = KSPCompiler.Domain.Symbols.MetaData.DataTypeUtility;

namespace KSPCompiler.ExternalSymbolRepository.Tsv.UITypes.Translators;

internal class FromTsvTranslator : IDataTranslator<string, IReadOnlyCollection<UITypeSymbol>>
{
    private enum Column
    {
        Name,
        Reserved,
        VariableType,
        Description,
        RequireInitializer,
        InitializerArgumentBegin,
    }

    private static readonly Regex LineComment = new( @"^#.*" );

    public IReadOnlyCollection<UITypeSymbol> Translate( string source )
    {
        var result = new List<UITypeSymbol>();

        TsvUtility.ParseTsv( source.SplitNewLine(), LineComment, values =>
            {
                TsvUtility.RemoveQuoteCharacter( values );

                var uiType = new UITypeSymbol( TsvUtility.ParseBoolean( values[ (int)Column.RequireInitializer ] ) )
                {
                    Name        = values[ (int)Column.Name ],
                    Reserved    = TsvUtility.ParseBoolean( values[ (int)Column.Reserved ] ),
                    Description = values[ (int)Column.Description ],
                    DataType    = DataTypeUtility.Guess( values[ (int)Column.VariableType ] )
                };

                ParseInitializerArguments( values, uiType );
                result.Add( uiType );
            }
        );

        return result;
    }

    private static void ParseInitializerArguments( string[] values, UITypeSymbol uiType )
    {
        /*
         * Argument1, Argument1Description, Argument2, Argument2Description, ...
         */
        TsvUtility.ParseColumnGroups( values, (int)Column.InitializerArgumentBegin, 2, arg =>
            {
                var argument = new UIInitializerArgumentSymbol
                {
                    Name        = arg[ 0 ],
                    Description = arg[ 1 ],
                    Reserved    = false
                };

                argument.DataType = DataTypeUtility.Guess( argument.Name );
                uiType.AddInitializerArgument( argument );
            }
        );
    }
}
