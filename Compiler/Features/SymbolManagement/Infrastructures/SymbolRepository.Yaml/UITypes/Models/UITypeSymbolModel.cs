using System;
using System.Collections.Generic;

using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace KSPCompiler.SymbolManagement.Repository.Yaml.UITypes.Models;

public sealed class UITypeSymbolModel : ISymbolModel
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public bool BuiltIn { get; set; }

    public string VariableType { get; set; } = string.Empty;

    [YamlMember( ScalarStyle = ScalarStyle.Literal )]
    public string Description { get; set; } = string.Empty;

    public string BuiltIntoVersion { get; set; } = string.Empty;

    public bool RequireInitializer { get; set; }

    public List<UITypeSymbolArgumentModel> InitializerArguments { get; set; } = [];

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
