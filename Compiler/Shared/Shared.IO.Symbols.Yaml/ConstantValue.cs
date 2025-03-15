using YamlDotNet.Serialization;

namespace KSPCompiler.Shared.IO.Symbols.Yaml;

public static class ConstantValue
{
    public static readonly ISerializer YamlSerializer = new SerializerBuilder().Build();
    public static readonly IDeserializer YamlDeserializer = new DeserializerBuilder().Build();
}
