using System.Diagnostics;
using System.IO;

using Antlr4.Runtime;

using KSPCompiler.Domain.Ast.Blocks;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Externals.Parser.Antlr.Translators;
using KSPCompiler.Gateways.Parsers;

namespace KSPCompiler.Externals.Parser.Antlr;

public abstract class KspScriptParser : IKspScriptParser
{
    protected Stream Stream { get; }
    public bool LeaveOpen { get; }

    public ICompilerMessageManger MessageManger { get; }

    protected KspScriptParser( Stream stream, ICompilerMessageManger messageManger, bool leaveOpen = false )
    {
        Stream        = stream;
        MessageManger = messageManger;
        LeaveOpen     = leaveOpen;
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
        var lexer = new KSPLexer( antlrStream );
        var tokenStream = new CommonTokenStream( lexer );
        var parser = new KSPParser( tokenStream );

        var lexerErrorListener = new LexerErrorListener( MessageManger );
        var parserErrorListener = new ParserErrorListener( MessageManger );

        lexer.AddErrorListener( lexerErrorListener );
        parser.AddErrorListener( parserErrorListener );

        var cst = parser.compilationUnit();

        if( lexerErrorListener.HasError || parserErrorListener.HasError )
        {
            throw new KspParserException( $"Syntax Invalid : {cst.exception}" );
        }

        var ast = cst.Accept( new CSTConverterVisitor() ) as AstCompilationUnit;
        Debug.Assert( ast != null );

        return ast;
    }
}