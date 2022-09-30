using System.IO;
using System.Text;

using KSPCompiler.Domain.CompilerMessages;

namespace KSPCompiler.Externals.Parser.Antlr;

public class KspStringSyntaxAnalyser : KspSyntaxAnalyser
{
    public KspStringSyntaxAnalyser( string script, ICompilerMessageManger messageManger, Encoding encoding )
        : base( new MemoryStream( encoding.GetBytes( script ) ), messageManger )
    {}
}
