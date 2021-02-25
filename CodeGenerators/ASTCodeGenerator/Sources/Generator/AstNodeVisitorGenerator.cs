using System.Collections.Generic;
using System.IO;

using KSPCompiler.Apps.ASTCodeGenerator.JsonModels;

namespace KSPCompiler.Apps.ASTCodeGenerator.Generator
{
    public class AstNodeVisitorGenerator : IAstNodeVisitorGenerator
    {
        public IAstNodeVisitorTextTransformer TextTransformer { get; }

        public AstNodeVisitorGenerator( IAstNodeVisitorTextTransformer transformer )
        {
            TextTransformer = transformer;
        }

        public void Generate( Setting setting, IList<AstNodesInfo> infoList )
        {
            var path = Path.Combine( setting.OutputDirectory, "IAstVisitor.cs" );

            File.WriteAllText(
                path,
                TextTransformer.TransformText( setting, infoList )
            );
        }
    }
}