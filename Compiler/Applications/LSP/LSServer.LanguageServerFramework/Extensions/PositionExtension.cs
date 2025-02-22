using KSPCompiler.Commons.Text;

namespace KSPCompiler.Applications.LSServer.LanguageServerFramework.Extensions;

using FrameworkPosition = EmmyLua.LanguageServer.Framework.Protocol.Model.Position;
using FrameworkRange = EmmyLua.LanguageServer.Framework.Protocol.Model.DocumentRange;

public static class PositionExtension
{
    public static Position As( this FrameworkPosition self )
    {
        return new Position
        {
            BeginLine   = self.Line + 1,
            BeginColumn = self.Character
        };
    }

    public static FrameworkPosition As( this Position self )
    {
        return new FrameworkPosition
        {
            Line      = self.BeginLine.Value - 1,
            Character = self.BeginColumn.Value
        };
    }

    public static FrameworkRange AsRange( this Position self )
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
