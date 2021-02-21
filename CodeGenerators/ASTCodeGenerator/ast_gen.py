import os
import sys
import json

import settings
import gen

def execute_gen( setting, ast_list, namespace_part, template_file, ouput_dir ):

    if len( ast_list ) <= 0:
        return

    if not ouput_dir:
        ouput_dir = setting.output_root_directory
    else:
        ouput_dir = os.path.join( setting.output_root_directory, ouput_dir)

    gen.generate_ast_all(
        setting,
        ast_list,
        namespace_part,
        template_file,
        ouput_dir )

def main( argv ):

    with open( "settings.json", encoding = "utf-8 ") as f:
        settings_json = json.load( f )
        setting = settings.Setting( settings_json )

    ast_default_list    = settings.load_ast_settings( "Default.json" )
    ast_block_list      = settings.load_ast_settings( "Block.json" )
    ast_statement_list  = settings.load_ast_settings( "Statement.json" )
    ast_expression_list = settings.load_ast_settings( "Expression.json" )

    execute_gen( setting, ast_default_list, None, "template/Default.cs", None )
    execute_gen( setting, ast_block_list, "Blocks", "template/Block.cs", setting.directory_block )
    execute_gen( setting, ast_statement_list, "Statements", "template/Statement.cs", setting.directory_statement )
    execute_gen( setting, ast_expression_list, "Expressions", "template/Expression.cs", setting.directory_expression )

    all_ast = ast_default_list + ast_block_list + ast_statement_list + ast_expression_list
    gen.generate_ast_id( setting, all_ast, "template/AstNodeId.cs", setting.output_root_directory )
    gen.generate_ast_visitor( setting, all_ast, "template/IAstVisitor.cs", setting.output_root_directory )

if __name__ == "__main__":
    main( sys.argv[1:] )
