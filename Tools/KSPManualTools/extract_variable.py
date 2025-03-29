import os.path
import time
import re
from typing import List


import requests
from bs4 import BeautifulSoup


OUTPUT_DIR = 'output'
OUTPUT_PREFIX = 'variables'

url_list: List[str] = [
    'https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/control-parameters',
    'https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/engine-parameters',
    'https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/zone-parameters',
    'https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/multi-script',
]

def read_html(url: str) -> str:
    """
    Read the HTML content from a URL or file.
    """
    if url.startswith('http'):
        response = requests.get(url)
        if response.status_code != requests.codes.ok:
            raise Exception(f"Failed to retrieve URL: {url} with status code: {response.status_code}")
        return response.text
    else:
        with open(url, 'r') as file:
            return file.read()


def main(argv: List[str]) -> None:
    if not os.path.exists(OUTPUT_DIR):
        os.makedirs(OUTPUT_DIR, exist_ok=True)

    regex_command = re.compile(r'\s*([\$|\%|\~|\?|\@|\!][A-Z0-9_]+)')
    variables: List[str] = []

    for url in url_list:

        print(f"Extracting from {url}...")

        html_text = read_html(url)

        soup = BeautifulSoup(html_text, 'html.parser')

        variable_elements = soup.find_all('code', {'class': 'code'})

        for x in variable_elements:

            variable_name: str = x.get_text(strip=True)
            variables_names = regex_command.findall(variable_name)

            if variables_names:
                for match in variables_names:
                    variables.append(match)

        time.sleep(2)

    # Remove duplicates
    # and sort the variables
    variables = list(dict.fromkeys(variables))
    output_path = os.path.join(OUTPUT_DIR, f'{OUTPUT_PREFIX}.txt')

    # Export the variables to a file
    with open(output_path, 'w') as file:
        for i in variables:
            file.write(f"{i}\n")

    print(f"Total variables extracted: {len(variables)}")

if __name__ == "__main__":
    import sys
    main(sys.argv[1:])
