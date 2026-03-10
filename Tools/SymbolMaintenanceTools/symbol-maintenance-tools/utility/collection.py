from typing import Any, Iterable, List


def collect_new_items(previous_items: List[str], new_items: List[str]) -> set[str]:
    """
    Collect new items that are present in new_items but not in previous_items.
    """

    if previous_items is None or len(previous_items) == 0:
        return set(new_items)

    return set(new_items) - set(previous_items)

def dump(items: Iterable[Any]) -> None:
    for item in items:
        print(item)
