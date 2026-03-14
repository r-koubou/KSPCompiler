import sys

from yaml_format import create_formatted_yaml
from ruamel.yaml import YAML

from typing import List

from yaml_format import append_single_quoted_scalar_string_symbol_started
from yaml_format import force_description_scalar_string


def validate_yaml(source_file: str, output_file: str):
    # Load source
    with open(source_file, "r", encoding="utf-8") as f:
        source = YAML().load(f)

    # sort the source by Name (Unicode code point order)
    source["Data"].sort(key=lambda x: x["Name"])

    # Append single quoted scalar string for values that start with specific symbols
    append_single_quoted_scalar_string_symbol_started(source)

    # Force literal scalar string for "Description" values
    force_description_scalar_string(source)

    # Write the formatted source to the output file
    with open(output_file, "w", encoding="utf-8") as f:
        yaml = create_formatted_yaml()
        yaml.dump(source, f)

def main(source_file: str, output_file: str):

    validate_yaml(source_file, output_file)
    print(f"{source_file}: sorted and written to {output_file}")

if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("Usage: python yaml_sort.py <source_file.yaml> [output_file.yaml]")
        sys.exit(1)

    source_file = sys.argv[1]

    # if output file is not provided, overwrite the source file
    if len(sys.argv) == 3:
        output_file = sys.argv[2]
    else:
        output_file = source_file

    main(source_file, output_file)
