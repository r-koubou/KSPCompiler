Symbol Maintenance Tools
========================

A set of tools for extracting the following information from the KSP Reference Manual:

- Variables
- Commands
- Callbacks
- UI

Any symbols with detected differences are manually reflected in the YAML files while checking the manual. (We would like to automate this process in the future.)

## Required Software

- [uv](https://github.com/astral-sh/uv)

## Setup

```bash
make setup
```

## Validate YAML Files

Validates the YAML files under `<project root>/Data/Symbols/`.

```bash
make validate_yaml
```

## Check for Duplicate UUIDs in YAML Files

Checks whether there are any duplicate UUIDs in the YAML files under `<project root>/Data/Symbols/`.

```bash
make validate_yaml_uuid
```

## Scraping and Generating Output Files

```bash
make gen
```

The generated files will be placed in `output/`.

### If git detects changes under `output/**`

- Update the YAML files under `<project root>/Data/Symbols/`.
  - Use [https://ksp-symbol-editor.pages.dev/](https://ksp-symbol-editor.pages.dev/) for editing.

