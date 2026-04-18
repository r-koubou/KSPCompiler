Ast Code Generator
==================

This is a code generator for generating C# code of AST classes for KSP compiler tools. It uses YAML files in the `definitions` directory as input, and generates C# code in the `out` directory.

Since this only generates the basic code, you can edit the generated code manually without any issues. If necessary, edit the definition file and regenerate the code.

## Required Software

- [uv](https://github.com/astral-sh/uv)

## Setup

```bash
uv sync
```

## Generation

```sh
uv run python main.py [--output-base-dir <output-base-dir>] [--ast-class-prefix <ast-class-prefix>]

options:
  -h, --help            show this help message and exit
  --output-base-dir OUTPUT_BASE_DIR
                        Output base directory (default: /Users/hiroaki/Develop/OSS/vsce/ksp/ksp-compiler-tools/ast-generator/out)
  --ast-class-prefix AST_CLASS_PREFIX
                        Prefix for AST node classes (default: Ast)
```
