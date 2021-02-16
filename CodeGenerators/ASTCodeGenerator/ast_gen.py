import os
import sys
import json

import settings
import gen


def main( argv ):

    with open( "settings.json", encoding = "utf-8 ") as f:
        settings_json = json.load( f )
        setting = settings.Setting( settings_json )

    ast_block_list      = settings.load_ast_settings( "Block.json" )
    ast_statement_list  = settings.load_ast_settings( "Statement.json" )
    ast_expression_list = settings.load_ast_settings( "Expression.json" )

    if len( ast_block_list ) > 0:
        gen.generate_ast_all(
            setting,
            ast_block_list,
            "Blocks",
            "template/Block.cs",
            os.path.join( setting.output_root_directory, setting.directory_block ) )

    if len( ast_statement_list ) > 0:
        gen.generate_ast_all(
            setting,
            ast_statement_list,
            "Statements",
            "template/Statement.cs",
            os.path.join( setting.output_root_directory, setting.directory_statement ) )

    if len( ast_expression_list ) > 0:
        gen.generate_ast_all(
            setting,
            ast_expression_list,
            "Expressions",
            "template/Expression.cs",
            os.path.join( setting.output_root_directory, setting.directory_expression ) )

    all_ast = ast_block_list + ast_statement_list + ast_expression_list
    gen.generate_ast_id( setting, all_ast, "template/AstNodeId.cs", setting.output_root_directory )
    gen.generate_ast_visitor( setting, all_ast, "template/IAstVisitor.cs", setting.output_root_directory )

if __name__ == "__main__":
    main( sys.argv[1:] )
