import os.path
import time
from typing import List


import requests
from bs4 import BeautifulSoup


OUTPUT_DIR = 'command'

if not os.path.exists(OUTPUT_DIR):
    os.makedirs(OUTPUT_DIR, exist_ok=True)


# html_text = ''

# with open('e.html', 'r') as file:
#     html_text = file.read()

url_list: List[List[str]] = [
    ['general-commands',                'https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/general-commands'],
    ['array-commands',                  'https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/array-commands'],
    ['engine-parameter-commands',       'https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/engine-parameter-commands'],
    ['event-commands',                  'https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/event-commands'],
    ['event-commands',                  'https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/event-commands'],
    ['group-commands',                  'https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/group-commands'],
    ['keyboard-commands',               'https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/keyboard-commands'],
    ['load-save-commands',              'https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/load-save-commands'],
    ['midi-object-commands',            'https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/midi-object-commands'],
    ['music-information-retrieval',     'https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/music-information-retrieval'],
    ['time-related-commands',           'https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/time-related-commands'],
    ['event-commands',                  'https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/event-commands'],
    ['user-interface-commands',         'https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/user-interface-commands'],
    ['zone-commands',                   'https://www.native-instruments.com/ni-tech-manuals/ksp-manual/en/zone-commands'],
]

for data in url_list:
    prefix      = data[0]
    url         = data[1]

    print(f"Extracting {prefix} from {url}...")

    response    = requests.get(url)
    html_text   = response.text

    soup = BeautifulSoup(html_text, 'html.parser')

    command_elements = soup.find_all('section', {'class': 'section'})
    commands: List[str] = []

    for x in command_elements:

        elements = x.find_all('th', {'colspan': '2'})

        if not elements:
            continue

        command_name: str = elements[0].get_text(strip=True)
        command_name = command_name.replace('<', '')
        command_name = command_name.replace('>', '')
        command_name = command_name.replace(' ', '')

        commands.append(command_name)

    commands = list(dict.fromkeys(commands))
    output_path = os.path.join(OUTPUT_DIR, f'{prefix}.txt')

    with open(output_path, 'w') as file:
        for i in commands:
            file.write(f"{i}\n")


    time.sleep(2)



