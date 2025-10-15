.PHONY: setup gen default
.DEFAULT_GOAL := default

setup:
	uv sync

gen: setup
	@echo "Variables..."
	uv run python extract_variable.py
	@echo "Commands..."
	uv run python extract_command.py

default:
	@echo "No default action."
	@exit 1
