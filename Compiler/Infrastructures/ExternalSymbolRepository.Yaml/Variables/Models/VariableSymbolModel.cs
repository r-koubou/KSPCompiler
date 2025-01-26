using System;

using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Variables.Models;

public sealed class VariableSymbolModel : ISymbolModel
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public bool BuiltIn { get; set; } = true;

    [YamlMember( ScalarStyle = ScalarStyle.Literal )]
    public string Description { get; set; } = string.Empty;

    public string BuiltIntoVersion { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
