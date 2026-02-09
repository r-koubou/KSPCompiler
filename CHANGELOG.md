CHANGELOG
=========

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
