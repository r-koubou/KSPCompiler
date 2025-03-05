using System.Collections.Generic;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.Symbol.IO.Tsv.UITypes.Models;

public sealed class UITypeModel
{
    public string Name { get; set; } = string.Empty;
    public bool BuiltIn { get; set; } = true;
    public string DataType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string BuiltIntoVersion { get; set; } = ConstantValue.DefaultBuiltIntoVersion;
    public bool InitializerRequired { get; set; } = false;
    public List<UITypeArgumentModel> Arguments { get; set; } = [];
}

public sealed class UITypeArgumentModel
{
    public string Name { set; get; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
