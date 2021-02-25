using System.Collections.Generic;
using System.Linq;

using KSPCompiler.Apps.ASTCodeGenerator.JsonModels;
using KSPCompiler.Apps.ASTCodeGenerator.Templates;

namespace KSPCompiler.Apps.ASTCodeGenerator.Generator
{
    public class AstNodeIdTextTransformer : IAstNodeIdTextTransformer
    {
        public string TransformText( Setting setting, IList<AstNodesInfo> infoList )
        {
            var classes = infoList.SelectMany( x => x.Classes ).ToList();
            var template = new AstNodeIdTemplate( setting.RootNamespace, classes );

            return template.TransformText();
        }
    }
}