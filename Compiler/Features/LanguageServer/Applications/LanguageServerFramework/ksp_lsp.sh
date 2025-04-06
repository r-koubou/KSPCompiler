#!/bin/bash

THIS_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)

dotnet "$THIS_DIR/KSPCompiler.Features.Applications.LanguageServer.LanguageServerFramework.dll" "$@"
