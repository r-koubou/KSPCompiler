using System.IO;

using Antlr4.Runtime;

using KSPCompiler.Domain.Events;
using KSPCompiler.Resources;

namespace KSPCompiler.Infrastructures.Parser.Antlr;

internal class AntlrLexerErrorListener : IAntlrErrorListener<int>
{
    private IEventDispatcher EventDispatcher { get; }
    public bool HasError { get; private set; }

    private bool EnableDetailMessage { get; }

    public AntlrLexerErrorListener( IEventDispatcher eventDispatcher, bool enableDetailMessage = true)
    {
        EventDispatcher     = eventDispatcher;
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

        EventDispatcher.Dispatch( new CompilationErrorEvent( message, line, charPositionInLine ) );
    }
}
