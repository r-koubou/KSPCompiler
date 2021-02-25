using System.Collections.Generic;
using System.Linq;

using KSPCompiler.Apps.ASTCodeGenerator.JsonModels;
using KSPCompiler.Apps.ASTCodeGenerator.Templates;

namespace KSPCompiler.Apps.ASTCodeGenerator.Generator
{
    public class AstNodeVisitorTextTransformer : IAstNodeVisitorTextTransformer
    {
        public string TransformText( Setting setting, IList<AstNodesInfo> infoList )
        {
            var classNames = infoList.SelectMany( info => info.Classes.Select( info.GetClassName ) );
            var template = new AstVisitorTemplate( setting.RootNamespace, classNames );

            return template.TransformText();
        }
    }
}