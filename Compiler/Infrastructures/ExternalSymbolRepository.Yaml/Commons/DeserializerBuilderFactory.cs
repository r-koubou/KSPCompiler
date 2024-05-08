using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Commons;

/// <summary>
/// <see cref="DeserializerBuilder"/> factory for this software specification.
/// </summary>
public static class DeserializerBuilderFactory
{
    public static DeserializerBuilder Create()
        => new DeserializerBuilder()
           .WithNamingConvention( PascalCaseNamingConvention.Instance );
}
