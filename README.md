KSPCompiler
===========

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)


[![Build KSP Compiler (develop)](https://github.com/r-koubou/KSPCompiler/actions/workflows/build_compiler_develop.yml/badge.svg?branch=develop)](https://github.com/r-koubou/KSPCompiler/actions/workflows/build_compiler_develop.yml) [![Build LSP Server (develop)](https://github.com/r-koubou/KSPCompiler/actions/workflows/build_lsp_server_develop.yml/badge.svg?branch=develop)](https://github.com/r-koubou/KSPCompiler/actions/workflows/build_lsp_server_develop.yml)


A compiler programs for **KONTAKT Script Processor** scripts.
This program is part of [Language support for NI KONTAKT(TM) Script Processor (KSP)](https://github.com/r-koubou/vscode-ksp).

## Architecture, Design

[![Ask DeepWiki](https://deepwiki.com/badge.svg)](https://deepwiki.com/r-koubou/KSPCompiler)
https://deepwiki.com/r-koubou/KSPCompiler

## Features

### Compilation

- Syntax analysis
- Semantic analysis
- Obfuscate

### Language server

- Completion
- Go to Definition
- Find all References
- Folding
- Hover
- Renaming
- Signature Help

### Built-in Symbol Management

- Variable
- UI Type
- Command
- Callback

Run as a standalone application

## Run as a standalone application


### Compilation

1. Build `Compiler/Applications/KSPCompilerCliApp`
2. Run command `kspc` showing usage information

### Built-in Symbol Management

1. Build `Compiler/Features/SymbolManagement/Applications/SymbolDatabaseCliApp`
2. Run shellscript to manage built-in symbol database (Yaml)


## Requirements

- .net 10.x
    - https://dotnet.microsoft.com/

## Limitations

- Extended syntax is not supported


## About KONTAKT

**KONTAKT** is registered trademarks of Native Instruments GmbH.

[https://www.native-instruments.com/](https://www.native-instruments.com/)
