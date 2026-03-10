import sys

from scrapling.spiders import Response
from .base_spider import BaseSpider

class UiTypeSpider(BaseSpider):
    """
    Scraping UI type signatures from Native Instruments KSP manual.

    Examples:

        spider = UiTypeSpider(url)
        result = spider.start()
        if spider_result.stats.failed_requests_count > 0:
            <error handling here>

    """

    name = "ui_type"

    def __init__(self, url: str):
        """
        ctor.

        Args:
            url: The URL to scrape
        """
        super().__init__(url)
        self.corrected_items = []
        """
        A list to store corrected UI type signatures extracted from the webpage.

        List Examples:

            "ui_button"
            "ui_knob"
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
                signature_text = signature_text.replace("declare ", "", 1)

                # Has constructor parameters, e.g., "ui_knob(x)"
                if "(" in signature_text:
                    # some command typo in document: missing ')'
                    if not signature_text.endswith(")"):
                        signature_text += ")"

                    signature_text = signature_text.replace("<", "")
                    signature_text = signature_text.replace(">", "")
                    signature_text = signature_text.replace(" ", "")
                # No constructor parameters, e.g., "ui_button"
                else:
                    signature_text = signature_text.replace("<", "")
                    signature_text = signature_text.replace(">", "")

                self.corrected_items.append(signature_text)

            yield

        # Remove duplicates and sort the list
        self.corrected_items = list(dict.fromkeys(self.corrected_items))
        self.corrected_items.sort()
