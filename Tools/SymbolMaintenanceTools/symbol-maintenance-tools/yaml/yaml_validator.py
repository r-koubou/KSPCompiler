import sys
import jsonschema

from  ruamel.yaml import YAML

from typing import List

def validate_yaml(schema_file: str, source_file: str):
    # Load schema
    with open(schema_file, "r", encoding="utf-8") as f:
        schema = YAML().load(f)

    # Load source
    with open(source_file, "r", encoding="utf-8") as f:
        source = YAML().load(f)

    # validate
    try:
        jsonschema.validate(instance=source, schema=schema)
    except:
        print(f"{source_file}: Validation failed.")
        raise


def main(args:List[str]):
    schema_file = args[0]
    source_file = args[1]

    validate_yaml(schema_file, source_file)
    print(f"{source_file}: Validation succeeded.")

if __name__ == "__main__":
    main(sys.argv[1:])
