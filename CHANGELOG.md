CHANGELOG
=========

## v1.1.0

### Language Server: Improvements to Completion

Added `--prefer-snippet-insertion` to the language server startup options.Added --prefer-snippet-insertion to the language server startup command arguments.
This allows snippets to be prioritized during code completion. Snippets streamline coding by providing placeholders for function and callback arguments.

### Bug Fixes

- Compilation error if a UI variable array has fewer elements than the initialization parameters
- Syntax and Semantic analysis

### Organized internal tools

- Organized python tools in the `/Tools` directory

## v1.0.4

### The Symbol Editor is now available on the web

- Editor: https://ksp-symbol-editor.pages.dev/
- Repository: https://github.com/r-koubou/ksp-symbol-editor

**NOTE**: CLI tools, such as the TSV import feature, will be deprecated in the future.

### Bug Fixes and Improvements

#### Added

- Added a symbol table for `PgsKeyId`
- Added a YAML schema file for symbol definitions
- Implemented argument count and data type validation for callback declarations
- Added data type checks for variables with UI modifiers
- Included PGS symbols in code completion

####  Changed

- Switched UI initialization argument handling from name-based to explicit `DataType` management
- Improved consistency checks between callback definitions and UI
- Adjusted YAML indentation and explicit string quoting rules
- Removed redundant execution in PR workflow
- Added temporary test CI setup

#### Fixed

- Fixed incorrect handling of PGS command arguments (previously treated as preprocessor symbols)
- Fixed reversed `expected` / `actual` values in error messages
- Fixed incorrect test data folder path
- Fixed case mismatch in csproj (UI → Ui)
- Fixed missing SET_CONDITION and RESET_CONDITION output during obfuscation
- Removed `CreatedAt` and `UpdatedAt` from YAML


## v1.0.3

### Bug fixes

- [#326 Fix Type Propagation Through Binary Expressions](https://github.com/r-koubou/KSPCompiler/pull/326) : Thank you to [cdbeckwith](https://github.com/cdbeckwith)

### Variables added

- $EVENT_PAR_OUTPUT_TYPE
- $EVENT_PAR_OUTPUT_INDEX

## v1.0.2

### KONTAKT 8.8 ready

#### Variables added

- $NI_EPP_EQ_MODE_ELECTRIC_C
- $NI_EPP_TREMOLO_MODE_ELECTRIC_P
- $ENGINE_PAR_EPP_ELECTRIC_C_BRILLIANT
- $ENGINE_PAR_EPP_ELECTRIC_C_SOFT
- $ENGINE_PAR_EPP_ELECTRIC_C_TREBLE
- $ENGINE_PAR_EPP_ELECTRIC_C_MEDIUM

## v1.0.1

### Language Server Implementation

Fix for DocumentSymbol (variable). Modified the editor's outline display to nest `DocumentSymbol` (variables) under the `on init` callback.

Before:

```
on init
variableA
variableB
variableC
```

After:

```
+ on init
    variableA
    variableB
    variableC
```


## v1.0.0

Initial release
