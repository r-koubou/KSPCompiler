using System.Collections.Generic;

using KSPCompiler.Apps.ASTCodeGenerator.JsonModels;

namespace KSPCompiler.Apps.ASTCodeGenerator.Generator
{
    public interface IAstNodeIdGenerator
    {
        IAstNodeIdTextTransformer TextTransformer { get; }

        void Generate( Setting setting, IList<AstNodesInfo> infoList );
    }
}