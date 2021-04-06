using KSPCompiler.Apps.ASTCodeGenerator.TemplateModels;

namespace KSPCompiler.Apps.ASTCodeGenerator.Templates
{
    public partial class InnerEnumTemplate
    {
        public TemplateContext Context { get; }
        public AstNodesInfo.Enum Source { get; }

        public InnerEnumTemplate( TemplateContext context, AstNodesInfo.Enum source )
        {
            Context = context;
            Source  = source;
        }
    }
}