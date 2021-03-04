using System.Collections.Generic;

using KSPCompiler.Apps.ASTCodeGenerator.Generator;
using KSPCompiler.Apps.ASTCodeGenerator.TemplateModels;

namespace KSPCompiler.Apps.ASTCodeGenerator
{
    public class Program
    {
        public static void Main( string[] args )
        {
            var setting = YamlFileSerializer.ReadYaml<Setting>( "Setting.yaml" );
            GenerateAst( setting );
        }

        private static void GenerateAst( Setting setting )
        {
            var astInfoList = new List<AstNodesInfo>();
            var blockNodes = GenerateAstNode( setting, "Block.yaml", new AstNodeGenerator( new AstNodeTransformer() ) );

            astInfoList.Add( blockNodes );

            new AstNodeIdGenerator( new AstNodeIdTextTransformer() )
               .Generate( setting, astInfoList );

            new AstNodeVisitorGenerator( new AstNodeVisitorTextTransformer() )
               .Generate( setting, astInfoList );
        }

        private static AstNodesInfo GenerateAstNode( Setting setting, string yamlFilePath, IAstNodeGenerator generator )
        {
            var nodeInfo = YamlFileSerializer.ReadYaml<AstNodesInfo>( yamlFilePath );
            generator.Generate( setting, nodeInfo );

            return nodeInfo;
        }
    }
}