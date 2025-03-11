using EmmyLua.LanguageServer.Framework.Protocol.Model.Diagnostic;

using KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Extensions;
using KSPCompiler.Features.Compilation.Gateways.EventEmitting;
using KSPCompiler.Shared.Text;

namespace KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.Compilation.Extensions;

public static class CompilationEventExtension
{
    private static Diagnostic CreateDiagnostic(
        Position position,
        string message,
        DiagnosticSeverity severity )
    {
        return new Diagnostic
        {
            Range    = position.AsRange(),
            Severity = severity,
            Message  = message
        };
    }

    public static Diagnostic AsDiagnostic( this CompilationErrorEvent self )
        => CreateDiagnostic( self.Position, self.Message, DiagnosticSeverity.Error );

    public static Diagnostic AsDiagnostic( this CompilationWarningEvent self )
        => CreateDiagnostic( self.Position, self.Message, DiagnosticSeverity.Warning );
}
