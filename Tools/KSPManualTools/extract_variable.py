import sys
import os.path
import re
from typing import List

from scrapling.spiders import Spider, Response

OUTPUT_DIR = 'output'
OUTPUT_PREFIX = 'variables'

OUTPUT_PATH = os.path.join(OUTPUT_DIR, f'{OUTPUT_PREFIX}.txt')

url_list: List[str] = [
    'https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/control-parameters',
    'https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/engine-parameters',
    'https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/zone-parameters',
    'https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/multi-script',
]

class VariableSpider(Spider):
    name = "command"

    def __init__(self):
        super().__init__()
        self.corrected_items = []
        self.start_urls = [
            'https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/control-parameters',
            'https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/engine-parameters',
            'https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/zone-parameters',
            'https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/multi-script',
        ]

    async def parse(self, response: Response):
        regex_variable = re.compile(r'\s*([\$|\%|\~|\?|\@|\!][A-Z0-9_]+)')

        # .code: per variable content
        for item in response.css(".code"):
            if item is None:
                continue

            # all variable signatures in the current item element
            variable_signatures = item.css("code::text").getall()

            if variable_signatures is None or len(variable_signatures) == 0:
                continue

            for handler in variable_signatures:
                signature_text = str(handler).strip()
                for x in regex_variable.findall(signature_text):
                    self.corrected_items.append(x)

            yield


def read_previous() -> List[str]:
    """
    Read previously extracted variables from a file.
    """
    result = []
    previous_path = OUTPUT_PATH
    if os.path.exists(previous_path):
        with open(previous_path, 'r') as file:
            result = [line.strip() for line in file.readlines()]
    return result

def main(argv: List[str]) -> None:
    if not os.path.exists(OUTPUT_DIR):
        os.makedirs(OUTPUT_DIR, exist_ok=True)

    variables: List[str] = []
    previous_variables: List[str] = read_previous()

    variable_spider = VariableSpider()
    result = variable_spider.start()

    if result.stats.failed_requests_count > 0:
        print(f"Failed to extract variables from {result.stats.failed_requests_count} requests.")
        sys.exit(1)

    variables = variable_spider.corrected_items
    # Remove duplicates
    # and sort the variables
    variables = list(dict.fromkeys(variables))
    variables.sort()
    output_path = OUTPUT_PATH

    # Export the variables to a file
    with open(output_path, 'w') as file:
        for i in variables:
            file.write(f"{i}\n")

    # Check for new variables
    if len(previous_variables) > 0:
        new_variables = set(variables) - set(previous_variables)
        if len(new_variables) > 0:
            print(f'New variables ({len(new_variables)}) found:')
            print('-' * 20)
            for variable in new_variables:
                print(variable)
            print('-' * 20)

    print(f"Total variables extracted: {len(variables)}")

if __name__ == "__main__":
    import sys
    main(sys.argv[1:])
