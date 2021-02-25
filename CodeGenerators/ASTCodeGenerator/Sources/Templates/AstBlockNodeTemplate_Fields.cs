namespace KSPCompiler.Apps.ASTCodeGenerator.Templates
{
    public partial class AstBlockNodeTemplate
    {
        public TemplateContext Context { get; }

        public AstBlockNodeTemplate( TemplateContext context )
        {
            Context = context;
        }
    }
}