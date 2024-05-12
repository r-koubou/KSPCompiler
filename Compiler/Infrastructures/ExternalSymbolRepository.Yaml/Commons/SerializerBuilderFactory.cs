using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Commons;

/// <summary>
/// <see cref="SerializerBuilderFactory"/> factory for this software specification.
/// </summary>
public static class SerializerBuilderFactory
{
    public static SerializerBuilder Create()
        => new SerializerBuilder()
           .WithNamingConvention( PascalCaseNamingConvention.Instance );
}
