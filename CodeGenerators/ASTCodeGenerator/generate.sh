#!/bin/bash

echo "# Generating AST source codes..."
dotnet run
if [ $? -eq 0 ]; then
    echo "done"
else
    echo "failed"
fi
