import sys
import re

from ruamel.yaml import YAML
from ruamel.yaml.scalarstring import SingleQuotedScalarString, LiteralScalarString

regex_symbol = re.compile(r"^[\{\[\-\?\:\,\|\>\!\"\'\#\&\*\%\@]")

def create_formatted_yaml() -> YAML:
    """
    Create a YAML instance with specific formatting options.
    Returns:
        YAML: A YAML instance with the specified formatting.
    """
    yaml = YAML()
    yaml.indent(mapping=2, sequence=4, offset=2)
    yaml.default_flow_style = False
    return yaml

def append_single_quoted_scalar_string_symbol_started(yaml_body):
    """
    Recursively traverse the YAML body and convert string values that start with specific symbols to SingleQuotedScalarString.

    Parameters:
        yaml_body (dict or list): The YAML body to process.
    """
    if isinstance(yaml_body, dict):
        for key, value in yaml_body.items():
            if isinstance(value, str) and regex_symbol.match(value):
                yaml_body[key] = SingleQuotedScalarString(value)
            else:
                append_single_quoted_scalar_string_symbol_started(value)
    elif isinstance(yaml_body, list):
        for index, item in enumerate(yaml_body):
            if isinstance(item, str) and regex_symbol.match(item):
                yaml_body[index] = SingleQuotedScalarString(item)
            else:
                append_single_quoted_scalar_string_symbol_started(item)

def force_description_scalar_string(yaml_body):
    """
    Recursively traverse the YAML body and convert string values of "Description" keys to LiteralScalarString.

    Parameters:
        yaml_body (dict or list): The YAML body to process.
    """
    if isinstance(yaml_body, dict):
        for key, value in yaml_body.items():
            if key == "Description":
                yaml_body[key] = LiteralScalarString(value)
            else:
                force_description_scalar_string(value)
    elif isinstance(yaml_body, list):
        for item in yaml_body:
            force_description_scalar_string(item)

def main(source_file: str, output_file: str):
    # Load source
    with open(source_file, "r", encoding="utf-8") as f:
        source = YAML().load(f)

    # Force single quoted scalar string for values that start with specific symbols
    append_single_quoted_scalar_string_symbol_started(source)

    # Force literal scalar string for Description values
    force_description_scalar_string(source)

    # Write the formatted source to the output file
    with open(output_file, "w", encoding="utf-8") as f:
        yaml = create_formatted_yaml()
        yaml.dump(source, f)

if __name__ == "__main__":

    if len(sys.argv) < 2:
        print("Usage: python yaml_format.py <source_file.yaml> [output_file.yaml]")
        sys.exit(1)

    source_file = sys.argv[1]

    # if output file is not provided, overwrite the source file
    if len(sys.argv) == 3:
        output_file = sys.argv[2]
    else:
        output_file = source_file


    main(source_file, output_file)