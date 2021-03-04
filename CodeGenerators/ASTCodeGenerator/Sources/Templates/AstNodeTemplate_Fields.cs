namespace KSPCompiler.Apps.ASTCodeGenerator.Templates
{
    public partial class AstNodeTemplate
    {
        public TemplateContext Context { get; }

        public AstNodeTemplate( TemplateContext context )
        {
            Context = context;
        }
    }
}