using System.IO;

using Antlr4.Runtime;

using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Resources;

namespace KSPCompiler.Infrastructures.Parser.Antlr;

internal class LexerErrorListener : IAntlrErrorListener<int>
{
    private ICompilerMessageManger MessageManger { get; }
    public bool HasError { get; private set; }

    private bool EnableDetailMessage { get; }

    public LexerErrorListener( ICompilerMessageManger messageManger, bool enableDetailMessage = true)
    {
        MessageManger       = messageManger;
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

        var compilerMessage = MessageManger.MessageFactory.Create(
            CompilerMessageLevel.Error,
            message,
            line,
            charPositionInLine
        );

        MessageManger.Append( compilerMessage );
    }
}
