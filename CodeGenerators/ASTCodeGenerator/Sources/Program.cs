using System;
using System.Collections.Generic;
using System.IO;

using KSPCompiler.Apps.ASTCodeGenerator.Generator;
using KSPCompiler.Apps.ASTCodeGenerator.TemplateModels;

namespace KSPCompiler.Apps.ASTCodeGenerator
{
    public class Program
    {
        public static void Main( string[] args )
        {
            var setting = YamlFileSerializer.ReadYaml<Setting>( "setting.yaml" );
            GenerateAst( setting );
        }

        private static void GenerateAst( Setting setting )
        {
            var astInfoList = new List<AstNodesInfo>();

            foreach( var definitionFile in setting.AstDefinitionFiles )
            {
                var astNodes = GenerateAstNode(
                    setting,
                    definitionFile,
                    new AstNodeGenerator( new AstNodeTransformer() )
                );

                astInfoList.Add( astNodes );

            }

            new AstNodeIdGenerator( new AstNodeIdTextTransformer() )
               .Generate( setting, astInfoList );

            new AstNodeVisitorGenerator( new AstNodeVisitorTextTransformer() )
               .Generate( setting, astInfoList );
        }

        private static AstNodesInfo GenerateAstNode( Setting setting, string yamlFilePath, IAstNodeGenerator generator )
        {
            if( !File.Exists( yamlFilePath ) )
            {
                Console.Error.WriteLine( $"Warning: {yamlFilePath} not found. skipped..." );
                return new AstNodesInfo();
            }

            var nodeInfo = YamlFileSerializer.ReadYaml<AstNodesInfo>( yamlFilePath );
            generator.Generate( setting, nodeInfo );

            return nodeInfo;
        }
    }
}