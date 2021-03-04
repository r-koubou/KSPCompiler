using System.Collections.Generic;

namespace KSPCompiler.Apps.ASTCodeGenerator.TemplateModels
{
    public class AstNodesInfo
    {
        public string Namespace { get; set; } = string.Empty;
        public IList<Class> Classes { get; set; } = new List<Class>();

        public string ClassNamePrefix { get; set; } = "Ast";
        public string ClassNameSuffix { get; set; } = "";

        public string SourceFilePrefix { get; set; } = "Ast";
        public string SourceFileSuffix { get; set; } = ".cs";

        public string GetSourceFileName( Class clazz )
            => $"{SourceFilePrefix}{clazz.Name}{SourceFileSuffix}";

        public string GetClassName( Class clazz )
            => $"{ClassNamePrefix}{clazz.Name}{ClassNameSuffix}";

        public string GetFullNamespace( Setting setting, AstNodesInfo info )
            => string.IsNullOrEmpty( info.Namespace )
                ? setting.RootNamespace
                : $"{setting.RootNamespace}.{info.Namespace}";

        public class Class
        {
            public string Description { get; set; } = string.Empty;
            public IList<string> Usings { get; set; } = new List<string>();
            public IList<string> Attributes { get; set; } = new List<string>();
            public string Name { get; set; } = string.Empty;
            public IList<string> BaseClasses { get; set; } = new List<string>();
            public IList<Field> Fields { get; set; } = new List<Field>();
            public bool HasConstructor { get; set; } = true;
            public string ConstructorSignature { get; set; } = string.Empty;
            public string ConstructorStatements { get; set; } = string.Empty;
            public bool HasAccept { get; set; } = false;
            public string AcceptStatements { get; set; } = string.Empty;
            public bool HasAcceptChildren { get; set; } = false;
            public string AcceptChildrenStatements { get; set; } = string.Empty;

            public class Field
            {
                public string Declaration { get; set; } = string.Empty;
                public string Description { get; set; } = string.Empty;
            }
        }
    }
}