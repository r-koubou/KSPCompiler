import sys
import os.path
import argparse

from .utility import collection
from .utility import textio

from .webscraping.base_spider import BaseSpider
from .webscraping.variable_scraping import VariableSpider
from .webscraping.command_scraping import CommandSpider
from .webscraping.callback_scraping import CallbackSpider
from .webscraping.ui_type_scraping import UiTypeSpider

from typing import Callable

THIS_MODULE_DIR = os.path.dirname(os.path.abspath(__file__))

PROJECT_ROOT_DIR =  os.path.join("..", os.path.dirname(THIS_MODULE_DIR))
CONFIG_DIR = os.path.join(PROJECT_ROOT_DIR, "config")

DEFAULT_OUTPUT_DIR = "out"

SEPARATOR_LINE1 = "=" * 40
SEPARATOR_LINE2 = "-" * 40


def execute(
    spider_factory: Callable[[str], BaseSpider],
    output_directory: str,
    url_list_file: str,
) -> None:
    """
    Execute the symbol extraction process.

    Args:
        spider_factory (Callable[[], BaseSpider]): A factory function that creates a new instance of a Spider for extracting symbols.
        output_directory (str): The directory where the extracted symbols will be saved.
        url_list_file (str): A text file containing the list of URLs to scrape for symbols, one URL per line.
    """

    all_symbols: list[str] = []
    all_previous_symbols: list[str] = []

    total_count = 0

    urls = textio.read_text_file_lines(url_list_file)

    for url in urls:
        spider = spider_factory(url)
        spider_result = spider.start()

        if spider_result.stats.failed_requests_count > 0:
            print(f"Failed to extract symbols from {url}")
            sys.exit(1)

        prefix = os.path.basename(url)
        output_path = os.path.join(output_directory, f"{prefix}.txt")

        previous = textio.read_text_file_lines(output_path)
        previous.sort()

        symbols = spider.corrected_items
        symbols = list(set(symbols))
        symbols.sort()

        total_count += len(symbols)

        textio.write_text_file_lines(output_path, symbols)

        all_previous_symbols.extend(previous)
        all_symbols.extend(symbols)

    # Check for new symbols
    new_symbols = collection.collect_new_items(all_previous_symbols, all_symbols)

    sorted_new_symbols = sorted(new_symbols)
    new_symbols_count = len(new_symbols)

    if new_symbols_count > 0:
        print(SEPARATOR_LINE2)

        # If there are fewer than 10 new symbols, print them all.
        # Otherwise, 10 new symbols + "..."
        if new_symbols_count < 10:
            for symbol in sorted_new_symbols:
                print(symbol)
        else:
            for symbol in sorted_new_symbols[:10]:
                print(symbol)
            print("...")

        print(SEPARATOR_LINE2)
        print(f"{new_symbols_count} New symbols found:")
    else:
        print("No new symbols found.")

    # Newline for better readability
    print("")


def parse_arguments(args: list[str]) -> argparse.Namespace:
    """
    Parse command-line arguments.

    Returns:
        argparse.Namespace: The parsed arguments.
    """
    parser = argparse.ArgumentParser(
        description="Scrape KSP manual pages to extract built-in symbols."
    )
    parser.add_argument(
        "--output-dir",
        type=str,
        default=DEFAULT_OUTPUT_DIR,
        help=f"The directory where the extracted symbols will be saved (default: {DEFAULT_OUTPUT_DIR}).",
    )
    return parser.parse_args(args)


# -------------------------------------------------------------------------------
# Entry point of the script
# -------------------------------------------------------------------------------


def main(args: list[str]):
    """
    Main function to execute the symbol extraction process.

    Args:
        args (list[str]): Command-line arguments passed to the script.
    """
    import logging

    logging.disable(logging.FATAL)
    parsed_args = parse_arguments(args)

    output_dir = parsed_args.output_dir
    os.makedirs(output_dir, exist_ok=True)

    execute_params = [
        {
            "name": "variable",
            "spider_factory": lambda url: VariableSpider(url),
            "output_directory": os.path.join(output_dir, "variable"),
            "url_list_file": os.path.join(CONFIG_DIR, "variable_urls.txt"),
        },
        {
            "name": "command",
            "spider_factory": lambda url: CommandSpider(url),
            "output_directory": os.path.join(output_dir, "command"),
            "url_list_file": os.path.join(CONFIG_DIR, "command_urls.txt"),
        },
        {
            "name": "callback",
            "spider_factory": lambda url: CallbackSpider(url),
            "output_directory": os.path.join(output_dir, "callback"),
            "url_list_file": os.path.join(CONFIG_DIR, "callback_urls.txt"),
        },
        {
            "name": "ui_type",
            "spider_factory": lambda url: UiTypeSpider(url),
            "output_directory": os.path.join(output_dir, "ui_type"),
            "url_list_file": os.path.join(CONFIG_DIR, "ui_type_urls.txt"),
        },
    ]

    for params in execute_params:
        print(SEPARATOR_LINE1)
        print(f"Extracting {params['name']} symbols...")
        print(SEPARATOR_LINE1)

        execute(
            spider_factory=params["spider_factory"],
            output_directory=params["output_directory"],
            url_list_file=params["url_list_file"],
        )


if __name__ == "__main__":
    main(sys.argv[1:])
