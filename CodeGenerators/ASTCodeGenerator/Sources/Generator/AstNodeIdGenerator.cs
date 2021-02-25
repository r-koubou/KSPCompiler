using System.Collections.Generic;
using System.IO;

using KSPCompiler.Apps.ASTCodeGenerator.JsonModels;

namespace KSPCompiler.Apps.ASTCodeGenerator.Generator
{
    public class AstNodeIdGenerator : IAstNodeIdGenerator
    {
        public IAstNodeIdTextTransformer TextTransformer { get; }

        public AstNodeIdGenerator( IAstNodeIdTextTransformer transformer )
        {
            TextTransformer = transformer;
        }

        public void Generate( Setting setting, IList<AstNodesInfo> infoList )
        {
            var path = Path.Combine( setting.OutputDirectory, "AstNodeId.cs" );

            File.WriteAllText(
                path,
                TextTransformer.TransformText( setting, infoList )
            );
        }
    }
}