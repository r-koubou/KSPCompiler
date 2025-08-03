import os.path
import time
import re
from typing import List


import requests
from bs4 import BeautifulSoup

URL_LIST: List[str] = [
    'https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/general-commands',
    'https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/array-commands',
    'https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/engine-parameter-commands',
    'https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/event-commands',
    'https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/group-commands',
    'https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/keyboard-commands',
    'https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/load-save-commands',
    'https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/midi-object-commands',
    'https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/music-information-retrieval',
    'https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/time-related-commands',
    'https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/user-interface-commands',
    'https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/zone-commands',
]

OUTPUT_DIR = os.path.join('output', 'command')

REGEX_COMMAND = re.compile(r'([a-zA-Z0-9_]+\([^\)]*\))')

def read_html(url: str) -> str:
    """
    Read the HTML content from a URL or file.
    """
    if url.startswith('http'):
        response = requests.get(url)
        if response.status_code != requests.codes.ok:
            raise Exception(f"Failed to retrieve URL: {url} with status code: {response.status_code}")
        return response
    else:
        with open(url, 'r') as file:
            return file.read()

def process(url: str, output_path: str) -> List[str]:
    print(f"Extracting from {url}...")

    response  = read_html(url)
    html_text = response.text

    soup = BeautifulSoup(html_text, 'html.parser')

    command_elements = soup.find_all('section', {'class': 'section'})
    commands: List[str] = []

    for x in command_elements:

        elements = x.find_all('th', {'data-priority': '1'})

        if not elements:
            continue

        command_name: str = elements[0].get_text(strip=True)
        command_name = command_name.replace('<', '')
        command_name = command_name.replace('>', '')
        command_name = command_name.replace(' ', '')

        command_names = REGEX_COMMAND.findall(command_name)

        if command_names:
            for match in command_names:
                commands.append(match)

    commands = list(dict.fromkeys(commands))

    with open(output_path, 'w') as file:
        for i in commands:
            file.write(f"{i}\n")

    return commands

def read_previous(file_path: str) -> List[str]:
    """
    Read previously extracted commands from a file.
    """
    result = []
    if os.path.exists(file_path):
        with open(file_path, 'r') as file:
            result = [line.strip() for line in file.readlines()]
            result = list(dict.fromkeys(result))
    return result

def main(argv: List[str]) -> None:
    if not os.path.exists(OUTPUT_DIR):
        os.makedirs(OUTPUT_DIR, exist_ok=True)

    all_commands: List[str] = []
    previous_all_commands: List[str] = []

    total_count = 0

    for url in URL_LIST:
        prefix      = os.path.basename(url)
        output_path = os.path.join(OUTPUT_DIR, f'{prefix}.txt')
        previous    = read_previous(output_path)
        commands    = process(url, output_path)
        total_count += len(commands)

        previous_all_commands.extend(previous)
        all_commands.extend(commands)
        time.sleep(2)

    # Check for new commands
    if len(previous_all_commands) > 0:
        new_commands = set(all_commands) - set(previous_all_commands)
        if len(new_commands) > 0:
            print(f'New commands ({len(new_commands)}) found:')
            print('-' * 20)
            for command in new_commands:
                print(command)
            print('-' * 20)

    print(f"Total commands extracted: {total_count}")

if __name__ == "__main__":
    import sys
    main(sys.argv[1:])
