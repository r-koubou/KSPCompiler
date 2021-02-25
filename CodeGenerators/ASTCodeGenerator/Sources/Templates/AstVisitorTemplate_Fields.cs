using System.Collections.Generic;

namespace KSPCompiler.Apps.ASTCodeGenerator.Templates
{
    public partial class AstVisitorTemplate
    {
        public string Namespace { get; }
        public IList<string> AstClassNameList { get; }

        public AstVisitorTemplate( string ns, IEnumerable<string> astClassName )
        {
            Namespace        = ns;
            AstClassNameList = new List<string>( astClassName );
        }
    }
}