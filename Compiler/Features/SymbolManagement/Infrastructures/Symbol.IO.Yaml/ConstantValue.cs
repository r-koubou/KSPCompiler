using YamlDotNet.Serialization;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Repository.Yaml;

public static class ConstantValue
{
    public static ISerializer YamlSerializer = new SerializerBuilder().Build();
    public static IDeserializer YamlDeserializer = new DeserializerBuilder().Build();
}
