using System.Collections.Generic;

namespace KSPCompiler.Apps.ASTCodeGenerator.TemplateModels
{
    public class AstNodesInfo
    {
        public string Namespace { get; set; } = string.Empty;
        public IList<Class> Classes { get; set; } = new List<Class>();
        public IList<Enum> Enums { get; set; } = new List<Enum>();

        public string ClassNamePrefix { get; set; } = "Ast";
        public string ClassNameSuffix { get; set; } = "";

        public string SourceFilePrefix { get; set; } = "Ast";
        public string SourceFileSuffix { get; set; } = ".cs";

        public string GetAstSourceFileName( Class clazz )
            => $"{SourceFilePrefix}{clazz.Name}{SourceFileSuffix}";

        public string GetAstClassName( Class clazz )
            => $"{ClassNamePrefix}{clazz.Name}{ClassNameSuffix}";

        public string GetSourceFileName( Class clazz )
            => $"{clazz.Name}{SourceFileSuffix}";

        public string GetClassName( Class clazz ) => clazz.Name;


        public string GetFullNamespace( Setting setting )
            => string.IsNullOrEmpty( Namespace )
                ? setting.RootNamespace
                : $"{setting.RootNamespace}.{Namespace}";

        public class Class
        {
            public IList<Class> InnerClasses { get; set; } = new List<Class>();
            public IList<Enum> InnerEnums { get; set; } = new List<Enum>();

            public bool Abstract { get; set; } = false;
            public string Description { get; set; } = string.Empty;
            public IList<string> Usings { get; set; } = new List<string>();
            public IList<string> Attributes { get; set; } = new List<string>();
            public string Name { get; set; } = string.Empty;
            public IList<string> BaseClasses { get; set; } = new List<string>();
            public IList<Field> Fields { get; set; } = new List<Field>();
            public IList<Constructor> Constructors { get; set; } = new List<Constructor>();
            public IList<Method> Methods { get; set; } = new List<Method>();
            public bool HasAccept { get; set; } = false;
            public string AcceptStatements { get; set; } = string.Empty;
            public bool HasAcceptChildren { get; set; } = false;
            public string AcceptChildrenStatements { get; set; } = string.Empty;

            public class Field
            {
                public string Description { get; set; } = string.Empty;
                public string Body { get; set; } = string.Empty;
            }

            public class Method
            {
                public string Description { get; set; } = string.Empty;
                public string Body { get; set; } = string.Empty;
            }

            public class Constructor
            {
                public string Body { get; set; } = string.Empty;
            }
        }

        public class Enum
        {
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public IList<string> Attributes { get; set; } = new List<string>();
            public IList<Field> Fields { get; set; } = new List<Field>();

            public class Field
            {
                public string Description { get; set; } = string.Empty;
                public string Name { get; set; } = string.Empty;
                public string Value { get; set; } = string.Empty;
            }
        }
    }
}