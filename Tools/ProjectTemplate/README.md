Project Template
================

This is a template project for generating csproj files.

## Required Software

- [uv](https://github.com/astral-sh/uv)

## Setup

```bash
make setup
```

## Generation

### Module

```bash
./usecase.sh <module name>
```

### Console Application

```bash
./cli.sh <application name>
```

### Use Case

```bash
./usecase.sh <usecase name>
```

The `csproj` and `Tests.csproj` (unit test project) files will be generated in the `out/` directory.
