using System.Collections.Generic;

using KSPCompiler.Apps.ASTCodeGenerator.Generator;
using KSPCompiler.Apps.ASTCodeGenerator.JsonModels;

namespace KSPCompiler.Apps.ASTCodeGenerator
{
    public class Program
    {
        public static void Main( string[] args )
        {
            var setting = JsonFileSerializer.ReadJson<Setting>( "Setting.json" );
            GenerateAst( setting );
        }

        private static void GenerateAst( Setting setting )
        {
            var astInfoList = new List<AstNodesInfo>();
            var blockNodes = GenerateAstNode( setting, "Block.json", new AstNodeGenerator( new AstBlockNodeTransformer() ) );

            astInfoList.Add( blockNodes );

            new AstNodeIdGenerator( new AstNodeIdTextTransformer() )
               .Generate( setting, astInfoList );

            new AstNodeVisitorGenerator( new AstNodeVisitorTextTransformer() )
               .Generate( setting, astInfoList );
        }

        private static AstNodesInfo GenerateAstNode( Setting setting, string jsonFilePath, IAstNodeGenerator generator )
        {
            var nodeInfo = JsonFileSerializer.ReadJson<AstNodesInfo>( jsonFilePath );
            generator.Generate( setting, nodeInfo );

            return nodeInfo;
        }

        // private static void GenerateAst( Setting setting )
        // {
        //     var astList = new List<AstNodesInfo>();
        //
        //     Directory.CreateDirectory( setting.OutputDirectory );
        //
        //     var blocks = GenerateAstBlock( setting );
        //
        //     astList.Add( blocks );
        //
        //     GenerateAstVisitor( setting, astList );
        //     GenerateAstIdEnum( setting, astList );
        // }
        //
        // private static AstNodesInfo GenerateAstBlock( Setting setting )
        // {
        //     var outputDirectory = setting.OutputDirectory;
        //     var asts = ReadJson<AstNodesInfo>( "Block.json" );
        //
        //     Directory.CreateDirectory( Path.Combine( outputDirectory, asts.Namespace ) );
        //
        //     foreach( var ast in asts.Classes )
        //     {
        //         var context = new TemplateContext( setting, asts, ast );
        //         var template = new BlockTemplate( context );
        //
        //         File.WriteAllText(
        //             BuildOutputPath( setting, asts, ast ),
        //             template.TransformText()
        //         );
        //     }
        //
        //     return asts;
        // }
        //
        // private static void GenerateAstVisitor( Setting setting, IList<AstNodesInfo> astList )
        // {
        //     var outputDirectory = setting.OutputDirectory;
        //     var classNames = astList.SelectMany( info => info.Classes.Select( info.GetClassName ) );
        //     var template = new AstVisitorTemplate( setting.RootNamespace, classNames );
        //
        //     var path = Path.Combine( outputDirectory, "IAstVisitor.cs" );
        //
        //     File.WriteAllText(
        //         path,
        //         template.TransformText()
        //     );
        // }
        //
        // private static void GenerateAstIdEnum( Setting setting, List<AstNodesInfo> astList )
        // {
        //     var outputDirectory = setting.OutputDirectory;
        //     var classes = astList.SelectMany( x => x.Classes ).ToList();
        //     var template = new AstNodeIdTemplate( setting.RootNamespace, classes );
        //
        //     var path = Path.Combine( outputDirectory, "AstNodeId.cs" );
        //
        //     File.WriteAllText(
        //         path,
        //         template.TransformText()
        //     );
        // }
        //
        // private static string BuildOutputPath( Setting setting, AstNodesInfo info, AstNodesInfo.Class ast )
        // {
        //     return Path.Combine(
        //         setting.OutputDirectory,
        //         info.Namespace,
        //         info.GetSourceFileName( ast )
        //     );
        // }
        //
        // private static T ReadJson<T>( string jsonFile )
        // {
        //     return JsonSerializer.Deserialize<T>(File.ReadAllText( jsonFile ) )
        //            ?? throw new ArgumentException($"{jsonFile} : Deserialize Failed" );
        // }
    }
}