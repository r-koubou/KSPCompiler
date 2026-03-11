KSP Manual Tools
================

# !DEPRECATED

- **This directory has been migrated to `../SymbolMaintenanceTools`.**
- **This directory is planned to be removed in the future.**

---

- **`../SymbolMaintenanceTools` にマイグレーションしました。**
- **このディレクトリは将来的に削除する予定です。**

<!--

A collection of tools that extract variable and command information from the KSP Reference Manual.
It parses the current documentation structure and generates data for use with the KSP Compiler.

## Required Software

- [uv](https://github.com/astral-sh/uv)

## Setup

```bash
make setup
```

## Generation

```bash
make gen
```

The `output/` directory will generate `variables.txt` and `*-commands.txt`.

### If git detects changes

 - Append or add the changes to the xlsx file located under `<project root>/Data/Symbols/` and output them in TSV format.
 - Run the import program located at `<project root>/Compiler/Features/SymbolManagement/Applications/SymbolDatabaseCliApp`.

## TODO

Since I, the developer, am the only one aware of the above manual operations, I plan to improve them in the future by implementing simpler management methods and automation.

-->
