@echo off

setlocal

set THIS_DIR=%~dp0

dotnet "%THIS_DIR%\KSPCompiler.Features.SymbolManagement.Applications.SymbolDbManager.dll" generate-template-callbacks %*

endlocal
