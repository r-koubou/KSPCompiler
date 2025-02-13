using System.IO;

using Antlr4.Runtime;

using KSPCompiler.Gateways.EventEmitting;
using KSPCompiler.Resources;

namespace KSPCompiler.Infrastructures.Parser.Antlr;

internal class AntlrLexerErrorListener : IAntlrErrorListener<int>
{
    private IEventEmitter EventEmitter { get; }
    public bool HasError { get; private set; }

    private bool EnableDetailMessage { get; }

    public AntlrLexerErrorListener( IEventEmitter eventEmitter, bool enableDetailMessage = true)
    {
        EventEmitter        = eventEmitter;
        EnableDetailMessage = enableDetailMessage;
    }

    public void SyntaxError( TextWriter output, IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e )
    {
        HasError = true;

        var message = CompilerMessageResources.syntax_error;

        if( EnableDetailMessage && !string.IsNullOrEmpty( msg ) )
        {
            message = string.Format( CompilerMessageResources.synax_error_detail, msg );
        }

        EventEmitter.Emit( new CompilationErrorEvent( message, line, charPositionInLine ) );
    }
}
