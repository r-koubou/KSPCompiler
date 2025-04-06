@echo off

setlocal

set THIS_KSPC_DIR=%~dp0

dotnet "%THIS_KSPC_DIR%\KSPCompiler.Features.SymbolManagement.Applications.SymbolDbManager.dll" delete-callbacks %*

endlocal
