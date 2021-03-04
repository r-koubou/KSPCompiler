using System.Collections.Generic;

using KSPCompiler.Apps.ASTCodeGenerator.TemplateModels;

namespace KSPCompiler.Apps.ASTCodeGenerator.Generator
{
    public interface IAstNodeTextTransformer
    {
        string TransformText( Setting setting, AstNodesInfo info, AstNodesInfo.Class clazz );
    }
}