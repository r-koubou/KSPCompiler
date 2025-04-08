using System;

using KSPCompiler.Features.LanguageServer.UseCase.Abstractions.Compilation;
using KSPCompiler.Shared.Text;
using KSPCompiler.Shared.UseCase;

namespace KSPCompiler.Features.LanguageServer.UseCase.Abstractions.SignatureHelp;

public sealed class SignatureHelpInputPortDetail(
    ICompilationCacheManager cache,
    ScriptLocation location,
    Position position )
{
    public ICompilationCacheManager Cache { get; } = cache;
    public ScriptLocation Location { get; } = location;
    public Position Position { get; } = position;
}

public sealed class SignatureHelpInputPort(
    SignatureHelpInputPortDetail input
) : InputPort<SignatureHelpInputPortDetail>( input );

public sealed class SignatureHelpOutputPort(
    SignatureHelpItem? signatureHelp,
    bool result,
    Exception? error = null
) : OutputPort<SignatureHelpItem?>( signatureHelp!, result, error );

public interface ISignatureHelpUseCase
    : IUseCase<SignatureHelpInputPort, SignatureHelpOutputPort>;
