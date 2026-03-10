import sys
import os.path
import utility.collection
import utility.textio

from webscraping.base_spider import BaseSpider
from webscraping.variable_scraping import VariableSpider
from webscraping.command_scraping import CommandSpider
from webscraping.callback_scraping import CallbackSpider
from webscraping.ui_type_scraping import UiTypeSpider

from scrapling.spiders import Spider


from typing import Callable, List


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

    all_symbols: List[str] = []
    all_previous_symbols: List[str] = []

    total_count = 0

    urls = utility.textio.read_text_file_lines(url_list_file)

    for url in urls:
        spider = spider_factory(url)
        spider_result = spider.start()

        if spider_result.stats.failed_requests_count > 0:
            print(f"Failed to extract symbols from {url}")
            sys.exit(1)

        prefix = os.path.basename(url)
        output_path = os.path.join(output_directory, f"{prefix}.txt")

        previous = utility.textio.read_text_file_lines(output_path)
        previous.sort()

        symbols = spider.corrected_items
        symbols = list(set(symbols))
        symbols.sort()

        total_count += len(symbols)

        utility.textio.write_text_file_lines(output_path, symbols)

        all_previous_symbols.extend(previous)
        all_symbols.extend(symbols)

    # Check for new symbols
    new_symbols = utility.collection.collect_new_items(
        all_previous_symbols, all_symbols
    )

    if len(new_symbols) > 0:
        print(f"New symbols ({len(new_symbols)}) found:")
        print("-" * 20)
        for symbol in new_symbols:
            print(symbol)
        print("-" * 20)
    else:
        print("No new symbols found.")


if __name__ == "__main__":

    import logging
    logging.disable(logging.FATAL)

    execute_params = [
        {
            "name": "variable",
            "spider_factory": lambda url: VariableSpider(url),
            "output_directory": os.path.join("output", "variable"),
            "url_list_file": "variable_urls.txt",
        },
        {
            "name": "command",
            "spider_factory": lambda url: CommandSpider(url),
            "output_directory": os.path.join("output", "command"),
            "url_list_file": "command_urls.txt",
        },
        {
            "name": "callback",
            "spider_factory": lambda url: CallbackSpider(url),
            "output_directory": os.path.join("output", "callback"),
            "url_list_file": "callback_urls.txt",
        },
        {
            "name": "ui_type",
            "spider_factory": lambda url: UiTypeSpider(url),
            "output_directory": os.path.join("output", "ui_type"),
            "url_list_file": "ui_type_urls.txt",
        },
    ]

    for params in execute_params:
        print("=" * 40)
        print(f"Extracting {params['name']} symbols...")
        print("=" * 40)

        execute(
            spider_factory=params["spider_factory"],
            output_directory=params["output_directory"],
            url_list_file=params["url_list_file"],
        )
