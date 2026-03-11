.PHONY: setup validate

setup:
	uv sync

PYTHON_SCRIPT_ROOT=./symbol-maintenance-tools
YAML_DIR=../../Data/Symbols

validate_yaml:
	echo "Starting YAML validation..."
	uv run python "$(PYTHON_SCRIPT_ROOT)/yaml/yaml_validator.py" "$(YAML_DIR)/variable-schema.yaml" "$(YAML_DIR)/variables.yaml"
	uv run python "$(PYTHON_SCRIPT_ROOT)/yaml/yaml_validator.py" "$(YAML_DIR)/command-schema.yaml" "$(YAML_DIR)/commands.yaml"
	uv run python "$(PYTHON_SCRIPT_ROOT)/yaml/yaml_validator.py" "$(YAML_DIR)/callback-schema.yaml" "$(YAML_DIR)/callbacks.yaml"
	uv run python "$(PYTHON_SCRIPT_ROOT)/yaml/yaml_validator.py" "$(YAML_DIR)/ui-type-schema.yaml" "$(YAML_DIR)/uitypes.yaml"
	echo "YAML validation completed successfully."

validate_yaml_uuid: validate_yaml
	echo "Starting YAML UUID validation..."
	uv run python "$(PYTHON_SCRIPT_ROOT)/yaml/uuid_validator.py" "$(YAML_DIR)/variables.yaml"
	uv run python "$(PYTHON_SCRIPT_ROOT)/yaml/uuid_validator.py" "$(YAML_DIR)/commands.yaml"
	uv run python "$(PYTHON_SCRIPT_ROOT)/yaml/uuid_validator.py" "$(YAML_DIR)/callbacks.yaml"
	uv run python "$(PYTHON_SCRIPT_ROOT)/yaml/uuid_validator.py" "$(YAML_DIR)/uitypes.yaml"
	echo "YAML UUID validation completed successfully."

gen:
	uv run python "$(PYTHON_SCRIPT_ROOT)/extract_symbols.py" output/
