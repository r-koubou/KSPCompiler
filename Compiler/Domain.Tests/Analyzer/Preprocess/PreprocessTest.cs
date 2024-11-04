using System;

using KSPCompiler.Domain.Ast.Analyzers;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;

using NUnit.Framework;

namespace KSPCompiler.Domain.Tests.Analyzer.Preprocess;

[TestFixture]
public class PreprocessTest
{
    [Test]
    public void TypicalPreprocessTest()
    {
        var analyzer = new PreprocessAnalyzer();
        var ast = new AstCompilationUnitNode();

        analyzer.Traverse( ast );
    }
}

#region Woprk mock classes
//
#endregion

public class PreprocessAnalyzer : DefaultAstVisitor, IAstTraversal
{
    public void Traverse( AstCompilationUnitNode node )
    {
        throw new NotImplementedException();
    }
}
