using YamlDotNet.Serialization;

namespace KSPCompiler.Shared.IO.Symbols.Yaml;

public static class ConstantValue
{
    public static ISerializer YamlSerializer = new SerializerBuilder().Build();
    public static IDeserializer YamlDeserializer = new DeserializerBuilder().Build();
}
