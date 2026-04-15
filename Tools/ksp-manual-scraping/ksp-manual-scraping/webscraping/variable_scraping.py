import re
import sys

from scrapling.spiders import Response
from .base_spider import BaseSpider

class VariableSpider(BaseSpider):
    """
    Scraping variable signatures from Native Instruments KSP manual.

    Examples:

        spider = VariableSpider(url)
        result = spider.start()
        if spider_result.stats.failed_requests_count > 0:
            <error handling here>

    """

    name = "variable"

    def __init__(self, url: str):
        """
        ctor.

        Args:
            urls: The list of URLs to scrape
        """
        super().__init__(url)
        self.corrected_items = []
        """
        A list to store corrected variable signatures extracted from the webpage.

        List Examples:

            "$CURRENT_SCRIPT_SLOT"
            "%GROUPS_SELECTED"
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
        regex_variable = re.compile(r'\s*([\$|\%|\~|\?|\@|\!][A-Z0-9_]+)')

        # .code: per variable content
        for item in response.css(".code"):
            if item is None:
                continue

            # all variable signatures in the current item element
            variable_signatures = item.css("code::text").getall()

            if variable_signatures is None or len(variable_signatures) == 0:
                continue

            for handler in variable_signatures:
                signature_text = str(handler).strip()
                for x in regex_variable.findall(signature_text):
                    self.corrected_items.append(x)

            yield

        # Remove duplicates and sort the list
        self.corrected_items = list(dict.fromkeys(self.corrected_items))
        self.corrected_items.sort()
