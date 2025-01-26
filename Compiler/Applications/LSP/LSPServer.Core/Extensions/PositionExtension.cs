using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace KSPCompiler.LSPServer.Core.Extensions;

using OmniSharpPosition = Position;
using OmniSharpRange = Range;

public static class PositionExtension
{
    public static OmniSharpPosition BeginAs( this Commons.Text.Position position )
    {
        return new OmniSharpPosition()
        {
            Line = position.BeginLine.Value - 1, // 1 origin to 0 origin
            Character = position.BeginColumn.Value,
        };
    }

    public static OmniSharpPosition EndAs( this Commons.Text.Position position )
    {
        return new OmniSharpPosition()
        {
            Line = position.EndLine.Value - 1, // 1 origin to 0 origin
            Character = position.EndColumn.Value,
        };
    }

    public static OmniSharpRange AsRange( this Commons.Text.Position position )
    {
        return new OmniSharpRange
        {
            Start = position.BeginAs(),
            End = position.EndAs()
        };
    }

}
