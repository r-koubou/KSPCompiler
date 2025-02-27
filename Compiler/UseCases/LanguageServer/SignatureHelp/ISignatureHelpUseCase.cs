using System;

using KSPCompiler.Commons.Text;
using KSPCompiler.UseCases.LanguageServer.Compilation;

namespace KSPCompiler.UseCases.LanguageServer.SignatureHelp;

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
    SignatureHelpInputPortDetail data
) : InputPort<SignatureHelpInputPortDetail>( data );

public sealed class SignatureHelpOutputPort(
    SignatureHelpItem? signatureHelp,
    bool result,
    Exception? error = null
) : OutputPort<SignatureHelpItem?>( signatureHelp!, result, error );

public interface ISignatureHelpUseCase
    : IUseCase<SignatureHelpInputPort, SignatureHelpOutputPort>;
