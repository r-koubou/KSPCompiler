using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

using NUnit.Framework;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.Tsv.Tests;

[TestFixture]
public class CsvHelperTest
{
    [Test]
    public void Test()
    {
        const string csv =
            """
            Name,Description,Argument1,Description1,Argument2,Description2
            message,send a message,argv,message to send,option, message option
            """;

        using var reader = new StringReader( csv );
        using var csvReader = new CsvReader( reader, new CsvConfiguration( CultureInfo.InvariantCulture )
            {
                HasHeaderRecord = true
            }
        );

        csvReader.Context.RegisterClassMap<RecordClassMap>();

        var records = csvReader.GetRecords<Record>();

        foreach( var x in records )
        {
            Console.WriteLine( $@"{x.Name} {x.Description}" );
        }
    }

    [Test]
    public void WriteTest()
    {
        const string csv =
            """
            Name	Description	Argument1	Description2	Argument2	Description2
            message	send a message	argv	message to send	option	message option
            message	send a message
            """;

        using var reader = new StringReader( csv );
        using var csvReader = new CsvReader( reader, new CsvConfiguration( CultureInfo.InvariantCulture )
            {
                HasHeaderRecord = true,
                Delimiter = "\t",
                MissingFieldFound = null
            }
        );

        csvReader.Context.RegisterClassMap<RecordClassMap>();

        var records = csvReader.GetRecords<Record>().ToList();

        using var writer = new StringWriter();
        using var csvWriter = new CsvWriter( writer, new CsvConfiguration( CultureInfo.InvariantCulture )
            {
                HasHeaderRecord = false,
                Delimiter       = "\t"
            }
        );

        csvWriter.Context.RegisterClassMap<RecordClassMap>();

        // Header
        csvWriter.WriteField( nameof( Record.Name ) );
        csvWriter.WriteField( nameof( Record.Description ) );

        var maxArguments = records.Max( x => x.Arguments.Count );

        for( var i = 1; i <= maxArguments; i++ )
        {
            csvWriter.WriteField( $"{nameof( Argument )}{i}" );
            csvWriter.WriteField( $"{nameof( Argument.Description )}{i}" );
        }

        csvWriter.NextRecord();

        // Records

        csvWriter.WriteRecords( records );

        var exportedCsv = writer.ToString();

        Console.WriteLine( exportedCsv );
    }

    private class Record
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<Argument> Arguments { get; set; } = [];
    }

    private sealed class RecordClassMap : ClassMap<Record>
    {
        public RecordClassMap()
        {
            Map( x => x.Name ).Name( "Name" );
            Map( x => x.Description ).Name( "Description" );
            Map( x => x.Arguments ).Name( $"Argument1" ).TypeConverter<ArgumentConverter>();
        }
    }

    private class Argument
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    private class ArgumentConverter : DefaultTypeConverter
    {
        public override object ConvertFromString( string? text, IReaderRow row, MemberMapData memberMapData )
        {
            var values = new List<Argument>();
            var header = row.Context.Reader?.HeaderRecord;

            if( header == null )
            {
                return values;
            }

            var startIndex = Array.IndexOf( header, "Argument1" );

            if( startIndex == -1 )
            {
                return values;
            }

            for( var i = startIndex; i < row.Parser.Count; i += 2 )
            {
                var name = row.GetField( i + 0 )!;
                var description = row.GetField( i + 1 ) ?? string.Empty;

                if( string.IsNullOrEmpty( name ) )
                {
                    break;
                }

                values.Add( new Argument
                    {
                        Name        = name,
                        Description = description
                    }
                );
            }

            return values;
        }

        public override string ConvertToString( object? value, IWriterRow row, MemberMapData memberMapData )
        {
            var arguments = (List<Argument>)value!;

            foreach( var argument in arguments )
            {
                row.WriteField( argument.Name );
                row.WriteField( argument.Description );
            }

            return string.Empty;
        }
    }
}
