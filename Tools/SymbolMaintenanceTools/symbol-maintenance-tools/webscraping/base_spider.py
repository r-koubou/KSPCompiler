import sys

from typing import List
from scrapling.spiders import Spider, Response

class BaseSpider(Spider):
    """
    Base of all spiders for scraping symbols from Native Instruments KSP manual.

    This class has collected_items property to store scraped items, and clear_collected_items method to clear the collected items before starting a new scraping session.
    """

    name = "command"

    def __init__(self, url: str):
        """
        ctor.

        Args:
            url: The URL to scrape
        """
        super().__init__()
        self.corrected_items = []
        """
        A list to store corrected command signatures extracted from the webpage.

        List Examples:

            "message(value)"
            "int(x)"
            :
            :
        """

        self.start_urls = [url]

    def clear_collected_items(self) -> None:
        """
        Clear the collected items before starting a new scraping session.
        """
        self.corrected_items = []
