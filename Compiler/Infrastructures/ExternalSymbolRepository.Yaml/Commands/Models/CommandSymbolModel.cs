using System;
using System.Collections.Generic;

using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace KSPCompiler.ExternalSymbolRepository.Yaml.Commands.Models;

public sealed class CommandSymbolModel : ISymbolModel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool BuiltIn { get; set; } = true;

    [YamlMember( ScalarStyle = ScalarStyle.Literal )]
    public string Description { get; set; } = string.Empty;

    public string BuiltIntoVersion { get; set; } = string.Empty;

    public string ReturnType { get; set; } = string.Empty;

    public List<CommandArgumentModel> Arguments { get; set; } = new();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
