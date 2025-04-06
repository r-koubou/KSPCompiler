@echo off

setlocal

set THIS_DIR=%~dp0

dotnet "%THIS_DIR%\KSPCompiler.Features.Compilation.Applications.Compilation.Cli.dll" %*

endlocal
