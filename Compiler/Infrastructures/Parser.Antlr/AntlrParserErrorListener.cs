using System.IO;

using Antlr4.Runtime;

using KSPCompiler.Domain.Events;
using KSPCompiler.Resources;

namespace KSPCompiler.Infrastructures.Parser.Antlr;

internal class AntlrParserErrorListener : IAntlrErrorListener<IToken>
{
    private IEventEmitter EventEmitter { get; }

    public bool HasError { get; private set; }

    private bool EnableDetailMessage { get; }


    public AntlrParserErrorListener( IEventEmitter eventEmitter, bool enableDetailMessage = true )
    {
        EventEmitter     = eventEmitter;
        EnableDetailMessage = enableDetailMessage;
    }

    public void SyntaxError( TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e )
    {
        HasError = true;

        var message = CompilerMessageResources.syntax_error;

        if( EnableDetailMessage && !string.IsNullOrEmpty( msg ) )
        {
            message = string.Format( CompilerMessageResources.synax_error_detail, msg );
        }

        EventEmitter.Dispatch( new CompilationErrorEvent( message, line, charPositionInLine ) );
    }
}
