using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace KSPCompiler.Apps.ASTCodeGenerator.JsonModels
{
    public class AstNodesInfo
    {
        public string Namespace { get; set; } = string.Empty;
        public IList<Class> Classes { get; set; } = new List<Class>();

        public string ClassNamePrefix { get; set; } = "Ast";
        public string ClassNameSuffix { get; set; } = "";

        public string SourceFilePrefix { get; set; } = "Ast";
        public string SourceFileSuffix { get; set; } = ".cs";

        public string GetSourceFileName( AstNodesInfo.Class clazz )
            => $"{SourceFilePrefix}{clazz.Name}{SourceFileSuffix}";

        public string GetClassName( AstNodesInfo.Class clazz )
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
            public IList<string> ConstructorStatements { get; set; } = new List<string>();
            public bool HasAccept { get; set; } = false;
            public IList<string> AcceptStatements { get; set; } = new List<string>();
            public bool HasAcceptChildren { get; set; } = false;
            public IList<string> AcceptChildrenStatements { get; set; } = new List<string>();

            public class Field
            {
                public string Declaration { get; set; } = string.Empty;
                public IList<string> Description { get; set; } = new List<string>();
            }
        }
    }
}