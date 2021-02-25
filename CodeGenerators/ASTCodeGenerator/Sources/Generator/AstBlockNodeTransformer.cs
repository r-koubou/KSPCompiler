using KSPCompiler.Apps.ASTCodeGenerator.JsonModels;
using KSPCompiler.Apps.ASTCodeGenerator.Templates;

namespace KSPCompiler.Apps.ASTCodeGenerator.Generator
{
    public class AstBlockNodeTransformer : IAstNodeTextTransformer
    {
        public string TransformText( Setting setting, AstNodesInfo info, AstNodesInfo.Class clazz )
        {
            var context = new TemplateContext( setting, info, clazz );
            var template = new AstBlockNodeTemplate( context );

            return template.TransformText();
        }
    }
}