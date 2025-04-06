import os.path
from pathlib import Path

THIS_DIR = Path(__file__).parent.resolve()

# コマンド名のリスト
COMMANDS = [
    "delete-callbacks",
    "delete-commands",
    "delete-ui-types",
    "delete-variables",
    "export-callbacks",
    "export-commands",
    "export-ui-types",
    "export-variables",
    "generate-template-callbacks",
    "generate-template-commands",
    "generate-template-ui-types",
    "generate-template-variables",
    "import-callbacks",
    "import-commands",
    "import-ui-types",
    "import-variables"
]

# テンプレート
TEMPLATE_BASH = """#!/bin/bash

THIS_DIR=$(cd "$(dirname "${{BASH_SOURCE[0]}}")" && pwd)

dotnet "$THIS_DIR/KSPCompiler.Features.SymbolManagement.Applications.SymbolDbManager.dll" {command} "$@"
"""

TEMPLATE_CMD = r"""@echo off

setlocal

set THIS_DIR=%~dp0

dotnet "%THIS_DIR%\KSPCompiler.Features.SymbolManagement.Applications.SymbolDbManager.dll" {command} %*

endlocal
"""

# 出力ディレクトリ
output_dir = Path(os.path.join(THIS_DIR, ".out"))
output_dir.mkdir(parents=True, exist_ok=True)

# ファイルを生成
def generate(template: str, suffix: str, newline: str = "\n"):
    print(f"Generating {suffix} files...")
    for cmd in COMMANDS:
        filename = f"db_{cmd.replace('-', '_')}.{suffix}"
        content = template.format(command=cmd)
        (output_dir / filename).write_text(content, newline=newline)



if __name__ == "__main__":
    generate(TEMPLATE_BASH, "sh", "\n")
    generate(TEMPLATE_CMD, "bat", "\r\n")
