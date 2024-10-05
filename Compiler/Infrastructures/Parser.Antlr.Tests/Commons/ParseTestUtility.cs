using System.IO;
using System.Text;

using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Infrastructures.Parser.Antlr;

using NUnit.Framework;

namespace KSPCompiler.Parser.Antlr.Tests.Commons;

public static class ParseTestUtility
{
    public static AstCompilationUnit Parse( string scriptDirectoryPath, string scriptFilePath )
    {
        var path = Path.Combine(
            TestContext.CurrentContext.TestDirectory,
            scriptDirectoryPath,
            scriptFilePath
        );

        var parser = new AntlrKspFileSyntaxParser( path, ICompilerMessageManger.Default, Encoding.UTF8 );
        var ast = parser.Parse();
        return ast;
    }
}
