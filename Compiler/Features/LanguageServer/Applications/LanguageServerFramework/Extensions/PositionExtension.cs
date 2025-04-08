using EmmyLua.LanguageServer.Framework.Protocol.Model;

namespace KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Extensions;

using FrameworkPosition = Position;
using FrameworkRange = DocumentRange;

public static class PositionExtension
{
    public static Shared.Text.Position As( this FrameworkPosition self )
    {
        return new Shared.Text.Position
        {
            BeginLine   = self.Line + 1,
            BeginColumn = self.Character
        };
    }

    public static FrameworkPosition As( this Shared.Text.Position self )
    {
        return new FrameworkPosition
        {
            Line      = self.BeginLine.Value - 1,
            Character = self.BeginColumn.Value
        };
    }

    public static FrameworkRange AsRange( this Shared.Text.Position self )
    {
        return new FrameworkRange
        {
            Start = new FrameworkPosition(
                self.BeginLine.Value - 1,
                self.BeginColumn.Value
            ),
            End = new FrameworkPosition(
                self.EndLine.Value - 1,
                self.EndColumn.Value
            )
        };
    }
}
