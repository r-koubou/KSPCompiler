using System.IO;

using Antlr4.Runtime;

using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Resources;

namespace KSPCompiler.Infrastructures.Parser.Antlr;

internal class ParserErrorListener : IAntlrErrorListener<IToken>
{
    private ICompilerMessageManger MessageManger { get; }

    public bool HasError { get; private set; }

    private bool EnableDetailMessage { get; }


    public ParserErrorListener( ICompilerMessageManger messageManger, bool enableDetailMessage = true )
    {
        MessageManger       = messageManger;
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

        var compilerMessage = MessageManger.MessageFactory.Create(
            CompilerMessageLevel.Error,
            message,
            line,
            charPositionInLine
        );

        MessageManger.Append( compilerMessage );
    }
}
