using System.IO;

using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Blocks;
using KSPCompiler.Shared.EventEmitting;

using NUnit.Framework;

namespace KSPCompiler.Features.Compilation.Infrastructures.Parser.Antlr.Tests.Commons;

public static class ParseTestUtility
{
    public static AstCompilationUnitNode Parse( string scriptDirectoryPath, string scriptFilePath )
    {
        var path = Path.Combine(
            TestContext.CurrentContext.TestDirectory,
            scriptDirectoryPath,
            scriptFilePath
        );

        var parser = new AntlrKspFileSyntaxParser( path, NullIEventEmitter.Instance );
        var ast = parser.Parse();
        return ast;
    }
}
