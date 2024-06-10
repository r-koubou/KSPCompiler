using System.IO;
using System.Text;

using KSPCompiler.Domain.CompilerMessages;

namespace KSPCompiler.Infrastructures.Parser.Antlr;

public class AntlrKspStringSyntaxAnalyzer : AntlrKspSyntaxAnalyzer
{
    public AntlrKspStringSyntaxAnalyzer( string script, ICompilerMessageManger messageManger, Encoding encoding )
        : base( new MemoryStream( encoding.GetBytes( script ) ), messageManger )
    {}
}
