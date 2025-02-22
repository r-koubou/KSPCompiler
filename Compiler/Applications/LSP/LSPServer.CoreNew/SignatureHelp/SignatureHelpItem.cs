using System.Collections.Generic;

namespace KSPCompiler.Applications.LSPServer.CoreNew.SignatureHelp;

public sealed record SignatureHelpItem
{
    public List<SignatureHelpInformationItem> Signatures { get; init; } = [];
    public int? ActiveSignature { get; init; }
    public int? ActiveParameter { get; init; }
}

public sealed record SignatureHelpInformationItem
{
    public string Label { get; init; } = null!;
    public StringOrMarkdownContent? Documentation { get; init; }
    public List<SignatureHelpParameterItem> Parameters { get; init; } = [];
    public int? ActiveParameter { get; init; }
}

public sealed record SignatureHelpParameterItem
{
    public string Label { get; init; } = null!;
    public StringOrMarkdownContent? Documentation { get; init; }
}
