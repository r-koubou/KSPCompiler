using System.Collections.Generic;

using KSPCompiler.Apps.ASTCodeGenerator.JsonModels;

namespace KSPCompiler.Apps.ASTCodeGenerator.Generator
{

    public interface IAstNodeGenerator
    {
        IAstNodeTextTransformer TextTransformer { get; }

        void Generate( Setting setting, AstNodesInfo info );

        void GenerateImpl( Setting setting, IList<AstNodesInfo> infoList )
        {
            foreach( var info in infoList )
            {
                Generate( setting, info );
            }
        }
    }
}