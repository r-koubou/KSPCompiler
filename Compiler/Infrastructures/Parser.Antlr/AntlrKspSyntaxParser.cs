using System.IO;

using Antlr4.Runtime;

using KSPCompiler.Domain;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Events;
using KSPCompiler.Gateways;
using KSPCompiler.Infrastructures.Parser.Antlr.Translators;

namespace KSPCompiler.Infrastructures.Parser.Antlr;

public abstract class AntlrKspSyntaxParser : ISyntaxParser
{
    protected Stream Stream { get; }
    public bool LeaveOpen { get; }
    public IEventDispatcher EventDispatcher { get; }

    protected AntlrKspSyntaxParser( Stream stream, IEventDispatcher eventDispatcher, bool leaveOpen = false )
    {
        Stream          = stream;
        EventDispatcher = eventDispatcher;
        LeaveOpen       = leaveOpen;
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

    public AstCompilationUnitNode Parse()
    {
        var antlrStream = new AntlrInputStream( Stream );
        var lexer = new KSPLexer( antlrStream, TextWriter.Null, TextWriter.Null );
        var tokenStream = new CommonTokenStream( lexer );
        var parser = new KSPParser( tokenStream, TextWriter.Null, TextWriter.Null );

        var lexerErrorListener = new AntlrLexerErrorListener( EventDispatcher );
        var parserErrorListener = new AntlrParserErrorListener( EventDispatcher );

        lexer.AddErrorListener( lexerErrorListener );
        parser.AddErrorListener( parserErrorListener );

        var cst = parser.compilationUnit();

        if( lexerErrorListener.HasError || parserErrorListener.HasError )
        {
            throw new KspScriptParseException( $"Syntax Invalid : {cst.exception}" );
        }

        var ast = cst.Accept( new CstConverterVisitor() ) as AstCompilationUnitNode;
        _ = ast ?? throw new MustBeNotNullException( nameof( ast ) );

        return ast;
    }
}
