using System.Text;

using KSPCompiler.Domain.Ast.Blocks;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Externals.Parser.Antlr;

using NUnit.Framework;

namespace KSPCompiler.Parser.Antlr.Tests;

[TestFixture]
public class AstTranslatorTest
{
    // ReSharper disable once UnusedMethodReturnValue.Local
    private static AstCompilationUnit TranslateImpl( string script )
    {
        var p = new KspStringScriptParser( script, ICompilerMessageManger.CreateDefault(), Encoding.UTF8 );
        return p.Parse();
    }

    [Test]
    public void LexerErrorTest()
    {
        const string script = @"
@@ @ @ - - - - on hoge( $arg1, $arg2,
end on
";
        Assert.Throws<KspParserException>( () => TranslateImpl( script ) );
    }

    [Test]
    public void ParserErrorTest()
    {
        const string script = @"
on hoge( $arg1, $arg2,
end on
";
        Assert.Throws<KspParserException>( () => TranslateImpl( script ) );
    }

    [Test]
    public void ExpressionTest()
    {
        const string script = @"
on hoge( $arg1, $arg2, $arg3 )
    $i := 0 + 1 + 2 + 91234h
    declare $i[100] (1,2,3,4)
    declare $j := 100
    declare ui_table $z(1,2,3)
end on
";
        TranslateImpl( script );
    }

    [Test]
    public void CallCommandTest()
    {
        const string script = @"
on hoge( $arg1, $arg2, $arg3 )
    command( -0 )
end on
";
        TranslateImpl( script );
    }

    [Test]
    public void AssignTest()
    {
        const string script = @"
on init
    x := x + 1
end on
";
        TranslateImpl( script );
    }

    [Test]
    public void StatementTest()
    {
        const string script = @"
on init
    if( a = 0 )
        command()
    end if
    select( x )
        case 0
            command()
        case 1 to 9
            command1to9()
    end select
    while( x < 10 )
        x := x + 1
    end while
    call userFunc
end on
";
        TranslateImpl( script );
    }

}