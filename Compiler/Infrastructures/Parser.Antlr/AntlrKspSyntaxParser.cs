using System.IO;

using Antlr4.Runtime;

using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Gateways.EventEmitting;
using KSPCompiler.Gateways.Parsers;
using KSPCompiler.Infrastructures.Parser.Antlr.Translators;

namespace KSPCompiler.Infrastructures.Parser.Antlr;

public abstract class AntlrKspSyntaxParser : ISyntaxParser
{
    protected Stream Stream { get; }
    public bool LeaveOpen { get; }
    public IEventEmitter EventEmitter { get; }

    protected AntlrKspSyntaxParser( Stream stream, IEventEmitter eventEmitter, bool leaveOpen = false )
    {
        Stream       = stream;
        EventEmitter = eventEmitter;
        LeaveOpen    = leaveOpen;
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

        var lexerErrorListener = new AntlrLexerErrorListener( EventEmitter );
        var parserErrorListener = new AntlrParserErrorListener( EventEmitter );

        lexer.AddErrorListener( lexerErrorListener );
        parser.AddErrorListener( parserErrorListener );

        var cst = parser.compilationUnit();
        var ast = cst.Accept( new CstConverterVisitor( tokenStream, EventEmitter ) ) as AstCompilationUnitNode;
        _ = ast ?? throw new MustBeNotNullException( nameof( ast ) );

        return ast;
    }
}
