using System.IO;

using KSPCompiler.Apps.ASTCodeGenerator.TemplateModels;

namespace KSPCompiler.Apps.ASTCodeGenerator.Generator
{
    public class AstNodeGenerator : IAstNodeGenerator
    {
        public IAstNodeTextTransformer TextTransformer { get; }

        public AstNodeGenerator( IAstNodeTextTransformer transformer )
        {
            TextTransformer = transformer;
        }

        public void Generate( Setting setting, AstNodesInfo info )
        {
            var outputDirectory = Path.Combine(
                setting.OutputDirectory,
                info.Namespace
            );

            Directory.CreateDirectory( outputDirectory );

            foreach( var ast in info.Classes )
            {
                File.WriteAllText(
                    GeneratorUtil.BuildOutputPath( setting, info, ast ),
                    TextTransformer.TransformText( setting, info, ast )
                );
            }
        }
    }
}