using System.Collections.Generic;

using KSPCompiler.Apps.ASTCodeGenerator.TemplateModels;

namespace KSPCompiler.Apps.ASTCodeGenerator.Generator
{
    public interface IAstNodeVisitorTextTransformer
    {
        string TransformText( Setting setting, IList<AstNodesInfo> infoList );
    }
}