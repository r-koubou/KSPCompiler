Project Template
================

This is a template project for generating csproj files.

## Required Software

- [uv](https://github.com/astral-sh/uv)

## Setup

```bash
uv sync
```

## Generation

```sh
uv run python main.py <template_path> <project_name> [--output-dir <output_dir>]

positional arguments:
  template_path         template file
  project_name          Project name used in the rendered csproj

options:
  -h, --help            show this help message and exit
  --output-dir OUTPUT_DIR
                        Output directory (default: _out)
```

## Generation aliases

### Module project

```bash
./module.sh <project name>
```

### Console Application project

```bash
./cli.sh <project name>
```

## Configuration

The `config.yaml` file contains the default values for the template variables. You can edit this file to change the default values.
