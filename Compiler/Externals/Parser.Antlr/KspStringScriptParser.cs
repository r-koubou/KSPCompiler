using System.IO;
using System.Text;

using KSPCompiler.Domain.CompilerMessages;

namespace KSPCompiler.Externals.Parser.Antlr;

public class KspStringScriptParser : KspScriptParser
{
    public KspStringScriptParser( string script, ICompilerMessageManger messageManger, Encoding encoding )
        : base( new MemoryStream( encoding.GetBytes( script ) ), messageManger )
    {}
}