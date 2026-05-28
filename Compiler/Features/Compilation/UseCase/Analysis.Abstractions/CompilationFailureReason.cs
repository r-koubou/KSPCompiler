namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions;

public enum CompilationFailureReason
{
    Other,
    IoError,
    SyntaxError,
    SemanticsError,
    SymbolNotFound
}
