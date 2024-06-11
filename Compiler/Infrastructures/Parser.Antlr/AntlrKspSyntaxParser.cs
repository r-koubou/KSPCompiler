using System.Diagnostics;
using System.IO;

using Antlr4.Runtime;

using KSPCompiler.Domain;
using KSPCompiler.Domain.Ast.Node.Blocks;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Gateways;
using KSPCompiler.Infrastructures.Parser.Antlr.Translators;

namespace KSPCompiler.Infrastructures.Parser.Antlr;

public abstract class AntlrKspSyntaxParser : ISyntaxParser
{
    protected Stream Stream { get; }
    public bool LeaveOpen { get; }
    public ICompilerMessageManger MessageManger { get; }

    private readonly TextWriter errorMessageWriter;

    protected AntlrKspSyntaxParser( Stream stream, ICompilerMessageManger messageManger, bool leaveOpen = false, TextWriter? errorMessageWriter = null )
    {
        Stream                  = stream;
        MessageManger           = messageManger;
        LeaveOpen               = leaveOpen;
        this.errorMessageWriter = errorMessageWriter ?? TextWriter.Null;
    }

    public void Dispose()
    {
        if( LeaveOpen )
        {
            return;
        }

        try
        {
            Stream.Close();
            Stream.Dispose();
        }
        catch
        {
            // ignored
        }
    }

    public AstCompilationUnit Parse()
    {
        var antlrStream = new AntlrInputStream( Stream );
        var lexer = new KSPLexer( antlrStream, TextWriter.Null, TextWriter.Null );
        var tokenStream = new CommonTokenStream( lexer );
        var parser = new KSPParser( tokenStream, TextWriter.Null, TextWriter.Null );

        var lexerErrorListener = new AntlrLexerErrorListener( MessageManger );
        var parserErrorListener = new AntlrParserErrorListener( MessageManger );

        lexer.AddErrorListener( lexerErrorListener );
        parser.AddErrorListener( parserErrorListener );

        var cst = parser.compilationUnit();

        if( lexerErrorListener.HasError || parserErrorListener.HasError )
        {
            MessageManger.WriteTo( errorMessageWriter );
            throw new KspScriptParseException( $"Syntax Invalid : {cst.exception}" );
        }

        var ast = cst.Accept( new CstConverterVisitor() ) as AstCompilationUnit;
        Debug.Assert( ast != null );

        return ast;
    }
}
