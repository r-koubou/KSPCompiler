import os.path
import time
import re
from typing import List

from scrapling.spiders import Spider, Response

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

class CommandSpider(Spider):
    name = "command"

    def __init__(self, url: str):
        super().__init__()
        self.corrected_items = []
        self.start_urls = [url]

    async def parse(self, response: Response):
        # .section: per command content
        for item in response.css(".section"):
            # th: command signature
            th = item.find("th")
            if th is None:
                continue

            # all command signatures in the current th element
            command_signatures = th.css("code::text").getall()

            if command_signatures is None or len(command_signatures) == 0:
                continue

            for handler in command_signatures:
                signature_text = str(handler).strip()
                if "(" in signature_text:
                    # some command typo in document: missing ')'
                    if not signature_text.endswith(")"):
                        signature_text += ")"

                    signature_text = signature_text.replace("<", "")
                    signature_text = signature_text.replace(">", "")
                    signature_text = signature_text.replace(" ", "")
                self.corrected_items.append(signature_text)

            yield

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
        spider = CommandSpider(url)
        spider_result = spider.start()

        if spider_result.stats.failed_requests_count > 0:
            print(f"Failed to extract commands from {url}")
            sys.exit(1)

        prefix      = os.path.basename(url)
        output_path = os.path.join(OUTPUT_DIR, f'{prefix}.txt')
        previous    = read_previous(output_path)
        commands    = spider.corrected_items

        commands    = list(set(commands))
        commands.sort()

        total_count += len(commands)

        with open(output_path, 'w') as file:
            for i in commands:
                file.write(f"{i}\n")

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
