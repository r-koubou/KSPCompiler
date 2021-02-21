import json

class Setting:
    def __init__( self, json ):
        self.root_namespace         = json[ "root_namespace" ]
        self.output_root_directory  = json[ "output_directory" ]
        self.directories            = json[ "directory_layout" ]
        self.directory_block        = self.directories[ "block" ]
        self.directory_statement    = self.directories[ "statement" ]
        self.directory_expression   = self.directories[ "expression" ]


class AstSetting:
    def __init__( self, json_element ):
        self.name        = json_element[ "name" ]
        self.base_list   = json_element.get( "base", ["IAstNode"] )
        self.using_list  = json_element.get( "using", [] )
        self.description = json_element.get( "description", "" )

def load_ast_settings( ast_setting_file ):
    with open( ast_setting_file, encoding = "utf-8" ) as f:
        ast_json = json.load( f )
        asts = []
        for x in ast_json :
            asts.append( AstSetting( x ) )

    return asts
