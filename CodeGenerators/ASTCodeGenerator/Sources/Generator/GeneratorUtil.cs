using System.IO;

using KSPCompiler.Apps.ASTCodeGenerator.TemplateModels;

namespace KSPCompiler.Apps.ASTCodeGenerator.Generator
{
    public static class GeneratorUtil
    {
        public static string BuildOutputPath( Setting setting, AstNodesInfo info, AstNodesInfo.Class ast )
        {
            return Path.Combine(
                setting.OutputDirectory,
                info.Namespace,
                info.GetAstSourceFileName( ast )
            );
        }

    }
}