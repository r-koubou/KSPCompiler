using YamlDotNet.Serialization;

namespace KSPCompiler.Features.Compilation.Infrastructures.ExternalSymbol.Yaml;

public static class ConstantValue
{
    public static ISerializer YamlSerializer = new SerializerBuilder().Build();
    public static IDeserializer YamlDeserializer = new DeserializerBuilder().Build();
}
