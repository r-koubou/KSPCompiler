namespace KSPCompiler.Features.LanguageServer.UseCase.Abstractions;

public enum LanguageServerFailureReason
{
    Other,
    IoError,
    Canceled,
    SymbolNotFound,
    SignatureNotFound
}
