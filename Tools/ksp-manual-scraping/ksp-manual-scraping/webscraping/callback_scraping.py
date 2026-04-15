import sys

from typing import List
from scrapling.spiders import Response
from .base_spider import BaseSpider

class CallbackSpider(BaseSpider):
    """
    Scraping callback signatures from Native Instruments KSP manual.

    Examples:

        spider = CallbackSpider(url)
        result = spider.start()
        if spider_result.stats.failed_requests_count > 0:
            <error handling here>

    """

    name = "callback"

    def __init__(self, url: str):
        """
        ctor.

        Args:
            url: The URL to scrape
        """
        super().__init__(url)
        self.corrected_items = []
        """
        A list to store corrected callback signatures extracted from the webpage.

        List Examples:

            "contoll"
            "init"
            "note"
            :
            :
        """

        self.start_urls = [url]

    def clear_collected_items(self) -> None:
        """
        Clear the collected items before starting a new scraping session.
        """
        self.corrected_items = []

    async def parse(self, response: Response):
        # .section: per callback content
        for item in response.css(".section"):
            # th: callback signature
            th = item.find("th")
            if th is None:
                continue

            # all callback signatures in the current th element
            callback_signatures = th.css("code::text").getall()

            if callback_signatures is None or len(callback_signatures) == 0:
                continue

            for handler in callback_signatures:
                signature_text = str(handler).strip()

                if signature_text.startswith("on "):
                    signature_text = signature_text[3:]

                self.corrected_items.append(signature_text)

            yield

        # Remove duplicates and sort the list
        self.corrected_items = list(dict.fromkeys(self.corrected_items))
        self.corrected_items.sort()
