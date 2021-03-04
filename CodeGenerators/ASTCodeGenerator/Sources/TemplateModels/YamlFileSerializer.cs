using System;
using System.IO;

using YamlDotNet.Serialization;

namespace KSPCompiler.Apps.ASTCodeGenerator.TemplateModels
{
    public static class YamlFileSerializer
    {
        public static T ReadYaml<T>( string yamlFile )
        {
            var yml = File.ReadAllText( yamlFile );
            var deserializer = new DeserializerBuilder()
                              //.WithNamingConvention( UnderscoredNamingConvention.Instance )
                              .Build();

            return deserializer.Deserialize<T>( yml )
                   ?? throw new ArgumentException($"{yamlFile} : Deserialize Failed" );
        }

    }
}