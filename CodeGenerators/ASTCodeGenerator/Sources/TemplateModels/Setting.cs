using System.Collections.Generic;

namespace KSPCompiler.Apps.ASTCodeGenerator.TemplateModels
{
    public class Setting
    {
        public string OutputDirectory { get; set; } = "out";
        public string RootNamespace { get; set; } = string.Empty;
        public IList<string> AstDefinitionFiles { get; set; } = new List<string>();
    }
}