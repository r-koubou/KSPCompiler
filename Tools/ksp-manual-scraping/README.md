KSP Manual Scraping
===================

## Overview

Scraping manual pages for Kontakt Script Processor (KSP) to extract built-in symbols.

## ⚠️ IMPORTANT NOTES ON WEB SCRAPING

Since the sole purpose of this program is to access the HTML source of the KSP manual to extract symbol information, it explicitly uses the `sleep` function to create intervals between requests; however, you should avoid making high-frequency requests.


## Required Software

- [uv](https://github.com/astral-sh/uv)

## List of URLs to Scrape

The list of URLs is stored in a text file within the `config` directory, with one URL per line.

## Setup

```sh
uv sync
```

## Usage

```sh
uv run python -m ksp-manual-scraping [-h] [--output-dir OUTPUT_DIR]

options:
  -h, --help            show this help message and exit
  --output-dir OUTPUT_DIR
                        The directory where the extracted symbols will be saved (default: output).
```

### Console Output Examples

```log
========================================
Extracting variable symbols...
========================================
----------------------------------------
$CONTROL_PAR_ACTIVE_INDEX
$CONTROL_PAR_ALLOW_AUTOMATION
$CONTROL_PAR_AUTOMATION_ID
$CONTROL_PAR_AUTOMATION_NAME
$CONTROL_PAR_BAR_COLOR
$CONTROL_PAR_BASEPATH
$CONTROL_PAR_BG_ALPHA
$CONTROL_PAR_BG_COLOR
$CONTROL_PAR_COLUMN_WIDTH
$CONTROL_PAR_CURSOR_PICTURE
...
----------------------------------------
1212 New symbols found:
```

`New symbols found` means the number of symbols that are newly extracted and not present in the existing output files.
This indicates how many new symbols have been added to the output compared to the previous run.
