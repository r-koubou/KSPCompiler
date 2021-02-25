using System.Collections.Generic;

using KSPCompiler.Apps.ASTCodeGenerator.JsonModels;

namespace KSPCompiler.Apps.ASTCodeGenerator.Generator
{
    public interface IAstNodeVisitorGenerator
    {
        IAstNodeVisitorTextTransformer TextTransformer { get; }

        void Generate( Setting setting, IList<AstNodesInfo> infoList );
    }
}