import sys
import os.path
import utility.collection
import utility.textio

from webscraping.variable_scraping import VariableSpider
from typing import List

def main(args: List[str]) -> None:

    output_dir = args[0]
    url_list_file = args[1]

    urls = utility.textio.read_text_file_lines(url_list_file)

    spider = VariableSpider(urls)
    spider_result = spider.start()

    if spider_result.stats.failed_requests_count > 0:
        print(f"Failed to extract variables from {urls}")
        sys.exit(1)

    output_path = os.path.join(output_dir, "variable", "variables.txt")

    previous = utility.textio.read_text_file_lines(output_path)
    previous.sort()

    variables = spider.corrected_items
    variables = list(set(variables))
    variables.sort()

    utility.textio.write_text_file_lines(output_path, variables)

    # Check for new variables
    new_variables = utility.collection.collect_new_items(previous, variables)

    if len(new_variables) > 0:
        print(f"New variables ({len(new_variables)}) found:")
        print("-" * 20)
        for variable in new_variables:
            print(variable)
        print("-" * 20)
    else:
        print("No new variables found.")

    print(f"Total variables extracted: {len(variables)}")


if __name__ == "__main__":
    if len(sys.argv) < 3:
        print("Usage: python extract_variable.py <output_directory> <url_list_file>")
        sys.exit(1)

    main(sys.argv[1:])
