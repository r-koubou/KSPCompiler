using System;
using System.IO;
using System.Text.Json;

namespace KSPCompiler.Apps.ASTCodeGenerator.JsonModels
{
    public static class JsonFileSerializer
    {
        public static T ReadJson<T>( string jsonFile )
        {
            return JsonSerializer.Deserialize<T>(File.ReadAllText( jsonFile ) )
                   ?? throw new ArgumentException($"{jsonFile} : Deserialize Failed" );
        }

    }
}