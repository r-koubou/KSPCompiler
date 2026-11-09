#!/bin/bash

uv run python main.py templates/module.csproj $1 --output-dir $2
uv run python main.py templates/test.csproj $1.Tests --output-dir $2
