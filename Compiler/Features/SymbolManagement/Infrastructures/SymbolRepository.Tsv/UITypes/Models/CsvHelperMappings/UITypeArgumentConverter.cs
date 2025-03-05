using System;
using System.Collections.Generic;

using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.ExternalSymbol.Tsv.UITypes.Models.CsvHelperMappings;

public sealed class UITypeArgumentConverter : DefaultTypeConverter
{
    public override object ConvertFromString( string? text, IReaderRow row, MemberMapData memberMapData )
    {
        var values = new List<UITypeArgumentModel>();
        var header = row.Context.Reader?.HeaderRecord;

        if( string.IsNullOrEmpty( text ) )
        {
            return values;
        }

        if( header == null )
        {
            return values;
        }

        var startIndex = Array.IndexOf( header, ConstantValue.ArgumentStartName );

        if( startIndex == -1 )
        {
            return values;
        }

        for( var i = startIndex; i < row.Parser.Count; i += 2 )
        {
            var name = row.GetField( i + 0 );
            var description = row.GetField( i + 1 ) ?? string.Empty;

            if( string.IsNullOrEmpty( name ) )
            {
                break;
            }

            values.Add( new UITypeArgumentModel
                {
                    Name        = name,
                    Description = description
                }
            );
        }

        return values;
    }

    public override string? ConvertToString( object? value, IWriterRow row, MemberMapData memberMapData )
    {
        var arguments = (List<UITypeArgumentModel>)value!;

        foreach( var argument in arguments )
        {
            row.WriteField( argument.Name );
            row.WriteField( argument.Description );
        }

        return string.Empty;
    }
}
