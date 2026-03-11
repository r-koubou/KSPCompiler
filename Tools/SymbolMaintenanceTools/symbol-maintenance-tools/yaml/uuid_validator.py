import sys

from typing import List
from ruamel.yaml import YAML


def validate_duplicate_uuid(source_file: str):
    # Load source
    with open(source_file, "r", encoding="utf-8") as f:
        source = YAML().load(f)

    # Check for duplicate UUIDs
    uuids = set()
    id_list: List[dict] = source.get("Data", [])

    if len(id_list) == 0:
        print(f"{source_file}: No data found in the YAML file.")
        return

    for item in id_list:
        uuid = item.get("Id")
        if uuid in uuids:
            raise ValueError(f"{source_file}: Duplicate UUID found: {uuid}")
        uuids.add(uuid)

def main(args:List[str]):
    source_file = args[0]

    validate_duplicate_uuid(source_file)
    print(f"{source_file}: All UUID(s) are unique.")

if __name__ == "__main__":
    main(sys.argv[1:])
