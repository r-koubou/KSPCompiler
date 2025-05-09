using System;
using System.Collections.Generic;

using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace KSPCompiler.Shared.IO.Symbols.Tsv.Callbacks.Models.CsvHelperMappings;

public sealed class CallbackArgumentConverter : DefaultTypeConverter
{
    public override object ConvertFromString( string? text, IReaderRow row, MemberMapData memberMapData )
    {
        var values = new List<CallbackArgumentModel>();
        var header = row.Context.Reader?.HeaderRecord;

        if( header == null )
        {
            return values;
        }

        var startIndex = Array.IndexOf( header, ConstantValue.ArgumentStartName );

        if( startIndex == -1 )
        {
            return values;
        }

        for( var i = startIndex; i < row.Parser.Count; i += 3 )
        {
            var name = row.GetField( i + 0 )!;
            var requiredDeclareOnInit = row.GetField( i + 1 ) ?? "false";
            var description = row.GetField( i + 2 ) ?? string.Empty;

            if( string.IsNullOrEmpty( name ) )
            {
                break;
            }

            values.Add( new CallbackArgumentModel
                {
                    Name                  = name,
                    RequiredDeclareOnInit = bool.Parse( requiredDeclareOnInit ),
                    Description           = description
                }
            );
        }

        return values;
    }

    public override string? ConvertToString( object? value, IWriterRow row, MemberMapData memberMapData )
    {
        var arguments = (List<CallbackArgumentModel>)value!;

        foreach( var argument in arguments )
        {
            row.WriteField( argument.Name );
            row.WriteField( argument.RequiredDeclareOnInit );
            row.WriteField( argument.Description );
        }

        return string.Empty;
    }
}
