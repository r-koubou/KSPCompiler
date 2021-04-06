using KSPCompiler.Apps.ASTCodeGenerator.TemplateModels;

namespace KSPCompiler.Apps.ASTCodeGenerator.Templates
{
    public partial class InnerClassTemplate
    {
        public TemplateContext Context { get; }
        public AstNodesInfo.Class Source { get; }

        public InnerClassTemplate( TemplateContext context, AstNodesInfo.Class source )
        {
            Context = context;
            Source  = source;
        }
    }
}