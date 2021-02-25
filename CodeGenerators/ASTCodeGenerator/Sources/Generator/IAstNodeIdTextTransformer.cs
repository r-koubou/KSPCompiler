using System.Collections.Generic;

using KSPCompiler.Apps.ASTCodeGenerator.JsonModels;

namespace KSPCompiler.Apps.ASTCodeGenerator.Generator
{
    public interface IAstNodeIdTextTransformer
    {
        string TransformText( Setting setting, IList<AstNodesInfo> infoList );
    }
}