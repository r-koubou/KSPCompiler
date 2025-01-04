using KSPCompiler.Commons.Text;

namespace KSPCompiler.LSPServer.Core.Extensions;

using OmniSharpPosition = OmniSharp.Extensions.LanguageServer.Protocol.Models.Position;

public static class PositionExtension
{
    public static OmniSharpPosition BeginAs( this Position position )
    {
        return new OmniSharpPosition()
        {
            Line = position.BeginLine.Value - 1, // 1 origin to 0 origin
            Character = position.BeginColumn.Value,
        };
    }

    public static OmniSharpPosition EndAs( this Position position )
    {
        return new OmniSharpPosition()
        {
            Line = position.EndLine.Value - 1, // 1 origin to 0 origin
            Character = position.EndColumn.Value,
        };
    }
}
