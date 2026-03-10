import sys
import os.path
import utility.collection
import utility.textio

from webscraping.command_scraping import CommandSpider
from typing import List

def main(args: List[str]) -> None:

    output_dir    = args[0]
    url_list_file = args[1]

    all_commands: List[str] = []
    all_previous_commands: List[str] = []

    total_count = 0

    urls = utility.textio.read_text_file_lines(url_list_file)

    for url in urls:
        spider = CommandSpider(url)
        spider_result = spider.start()

        if spider_result.stats.failed_requests_count > 0:
            print(f"Failed to extract commands from {url}")
            sys.exit(1)

        prefix = os.path.basename(url)
        output_path = os.path.join(output_dir, "command", f"{prefix}.txt")

        previous = utility.textio.read_text_file_lines(output_path)
        previous.sort()

        commands = spider.corrected_items
        commands = list(set(commands))
        commands.sort()

        total_count += len(commands)

        utility.textio.write_text_file_lines(output_path, commands)

        all_previous_commands.extend(previous)
        all_commands.extend(commands)

    # Check for new commands
    new_commands = utility.collection.collect_new_items(all_previous_commands, all_commands)

    if len(new_commands) > 0:
        print(f"New commands ({len(new_commands)}) found:")
        print("-" * 20)
        for command in new_commands:
            print(command)
        print("-" * 20)
    else:
        print("No new commands found.")

    print(f"Total commands extracted: {total_count}")

if __name__ == "__main__":
    if len(sys.argv) < 3:
        print("Usage: python extract_command.py <output_directory> <url_list_file>")
        sys.exit(1)

    main(sys.argv[1:])
