import os
from typing import List

def read_text_file_lines(file_path: str, encoding = "utf-8") -> List[str]:
    """
    Reads a text file and returns its lines as a list of strings.

    Args:
        file_path (str): The path to the text file.
        encoding (str, optional): The encoding of the text file. Defaults to "utf-8".

    Returns:
        If the file exists, a list of strings representing the lines in the file. Otherwise, an empty list.
    """
    if not os.path.exists(file_path):
        return []

    result: List[str] = []

    with open(file_path, "r", encoding=encoding) as f:
        for line in f.readlines():
            line = line.strip()
            if len(line) == 0:
                continue
            result.append(line)

    return result


def write_text_file_lines(file_path: str, lines: List[str], encoding = "utf-8") -> None:
    """
    Writes a list of strings to a text file, each string as a separate line.

    Args:
        file_path (str): The path to the text file.
        lines (List[str]): A list of strings to write to the file.
        encoding (str, optional): The encoding of the text file. Defaults to "utf-8".
    """
    dirctory_path = os.path.dirname(file_path)
    os.makedirs(dirctory_path, exist_ok=True)

    with open(file_path, "w", encoding=encoding) as f:
        for x in lines:
            f.write(f"{x}\n")
