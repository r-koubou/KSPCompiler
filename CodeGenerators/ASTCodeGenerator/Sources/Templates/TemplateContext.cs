using KSPCompiler.Apps.ASTCodeGenerator.TemplateModels;

namespace KSPCompiler.Apps.ASTCodeGenerator.Templates
{
    public class TemplateContext
    {
        public const string NoIndent = "";
        public const string Indent = "    ";
        public const string Indent2 = "        ";
        public const string Indent3 = "            ";
        public const string Indent4 = "                ";

        public Setting Setting { get; }
        public AstNodesInfo Info { get; }
        public AstNodesInfo.Class NodeClass { get; }

        public TemplateContext(
            Setting setting,
            AstNodesInfo info,
            AstNodesInfo.Class nodeClass )
        {
            Setting   = setting;
            Info      = info;
            NodeClass = nodeClass;
        }
    }
}