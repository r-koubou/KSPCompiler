using EmmyLua.LanguageServer.Framework.Protocol.Model.Diagnostic;

using KSPCompiler.Applications.LSServer.LanguageServerFramework.Extensions;
using KSPCompiler.Commons.Text;
using KSPCompiler.Gateways.EventEmitting;

namespace KSPCompiler.Applications.LSServer.LanguageServerFramework.Compilation.Extensions;

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
