import os
import sys
import json

class Setting:
    def __init__( self, json, root_namespace ):
        self.root_namespace         = root_namespace
        self.output_root_directory  = json[ "output_directory" ]
        self.directories            = json[ "directory_layout" ]
        self.directory_usecase      = self.directories[ "usecase" ]
        self.directory_inout        = self.directories[ "inout" ]
        self.directory_interactor   = self.directories[ "interactor" ]
        self.directory_presenter    = self.directories[ "presenter" ]

class UsecaseModel:
    def __init__( self, json_element ):
        self.name      = json_element[ "usecase_name" ]
        self.namespace = json_element[ "namespace" ]

def generate( setting, namespace, name, template_file, output_directory, prefix = "", suffix = "" ):
    with open( template_file, encoding="utf8" ) as f:
        template = f.read()

    code = template
    code = code.replace( "__namespace__", namespace )
    code = code.replace( "__name__", name )

    os.makedirs( output_directory, exist_ok = True )
    output_path = os.path.join( output_directory, prefix + name + suffix + ".cs" )

    with open( output_path, mode = "w", encoding = "utf8" ) as f:
        f.write( code )

def generate_all( setting, usecase ):

    namespace = lambda category: "{root}.{category}.{ns}".format(
        root = setting.root_namespace,
        category = category,
        ns = usecase.namespace )

    output_root_directory = setting.output_root_directory

    generate( setting, namespace("UseCases"),    usecase.name, "template/UseCase.cs", os.path.join( output_root_directory, setting.directory_usecase ), "I", "UseCase"  )
    generate( setting, namespace("UseCases"),    usecase.name, "template/Request.cs", os.path.join( output_root_directory, setting.directory_inout ), "", "Request"  )
    generate( setting, namespace("UseCases"),    usecase.name, "template/Response.cs", os.path.join( output_root_directory, setting.directory_inout ), "", "Response"  )
    generate( setting, namespace("Interactors"), usecase.name, "template/Interactor.cs", os.path.join( output_root_directory, setting.directory_interactor ), "", "Interactor"  )
    generate( setting, namespace("Presenters"),  usecase.name, "template/Presenter.cs", os.path.join( output_root_directory, setting.directory_presenter ), "I", "Presenter"  )


def main( argv ):

    with open( "classes.json", encoding = "utf-8" ) as f:
        classes_json = json.load( f )
        root_namespace = classes_json[ "root_namespace" ]
        usecases = []
        for x in classes_json[ "usecases" ] :
            usecases.append( UsecaseModel( x ) )

    with open( "settings.json", encoding = "utf-8 ") as f:
        settings_json = json.load( f )
        setting = Setting( settings_json, root_namespace )

    for x in usecases:
        print( x.name )
        generate_all( setting, x )

if __name__ == "__main__":
    main( sys.argv[1:] )
