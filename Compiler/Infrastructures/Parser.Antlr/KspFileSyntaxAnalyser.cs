using System.IO;
using System.Text;

using KSPCompiler.Domain.CompilerMessages;

namespace KSPCompiler.Infrastructures.Parser.Antlr;

public class KspFileSyntaxAnalyser : KspSyntaxAnalyser
{
    public KspFileSyntaxAnalyser( string path, ICompilerMessageManger messageManger, Encoding encoding )
        : base( File.OpenRead( path ), messageManger )
    {}
}
