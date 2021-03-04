using System.Collections.Generic;

using KSPCompiler.Apps.ASTCodeGenerator.TemplateModels;

namespace KSPCompiler.Apps.ASTCodeGenerator.Templates
{
    public partial class AstNodeIdTemplate
    {
        public string Namespace { get; }
        public IList<AstNodesInfo.Class> IdList { get; }

        public AstNodeIdTemplate( string ns, IEnumerable<AstNodesInfo.Class> idList )
        {
            Namespace = ns;
            IdList    = new List<AstNodesInfo.Class>( idList );
        }
    }
}