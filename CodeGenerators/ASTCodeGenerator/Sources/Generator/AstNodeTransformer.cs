using KSPCompiler.Apps.ASTCodeGenerator.TemplateModels;
using KSPCompiler.Apps.ASTCodeGenerator.Templates;

namespace KSPCompiler.Apps.ASTCodeGenerator.Generator
{
    public class AstNodeTransformer : IAstNodeTextTransformer
    {
        public string TransformText( Setting setting, AstNodesInfo info, AstNodesInfo.Class clazz )
        {
            var context = new TemplateContext( setting, info, clazz );
            var template = new AstNodeTemplate( context );

            return template.TransformText();
        }
    }
}