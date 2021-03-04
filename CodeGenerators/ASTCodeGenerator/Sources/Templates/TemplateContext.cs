using KSPCompiler.Apps.ASTCodeGenerator.TemplateModels;

namespace KSPCompiler.Apps.ASTCodeGenerator.Templates
{
    public class TemplateContext
    {
        public Setting Setting { get; }
        public AstNodesInfo Info { get; }
        public AstNodesInfo.Class AstNodeClass { get; }

        public TemplateContext(
            Setting setting,
            AstNodesInfo info,
            AstNodesInfo.Class nodeClass )
        {
            Setting      = setting;
            Info         = info;
            AstNodeClass = nodeClass;
        }
    }
}