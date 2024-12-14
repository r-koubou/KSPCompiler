using KSPCompiler.Domain.Events;

using OmniSharp.Extensions.LanguageServer.Protocol.Models;

using CompilerPosition = KSPCompiler.Commons.Text.Position;

namespace KSPCompiler.LSPServer.Core.Extensions;

public static class CompilationEventExtension
{
    private static Diagnostic CreateDiagnostic( CompilerPosition position, string message, DiagnosticSeverity severity )
    {
        return new Diagnostic
        {
            Range    = new Range
            {
                Start = new Position
                {
                    Line      = position.BeginLine.Value,
                    Character = position.BeginColumn.Value
                },
                End = new Position
                {
                    Line      = position.EndLine.Value,
                    Character = position.EndColumn.Value
                }
            },
            Severity = severity,
            Message  = message
        };
    }

    public static Diagnostic AsDiagnostic( this CompilationErrorEvent self )
        => CreateDiagnostic( self.Position, self.Message, DiagnosticSeverity.Error );

    public static Diagnostic AsDiagnostic( this CompilationWarningEvent self )
        => CreateDiagnostic( self.Position, self.Message, DiagnosticSeverity.Warning );

}
