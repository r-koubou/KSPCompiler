CHANGELOG
=========

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
