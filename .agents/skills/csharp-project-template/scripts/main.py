import argparse
import os
import os.path
import sys

from ruamel.yaml import YAML
from jinja2 import Template

THIS_SCRIPT_DIR = os.path.dirname(os.path.abspath(__file__))

CONFIG_FILE = os.path.join(THIS_SCRIPT_DIR, "config.yaml")
DEFAULT_OUTPUT_DIR = "out"

yaml = YAML()


def load_config() -> dict:
    with open(CONFIG_FILE, "r", encoding="utf-8") as file:
        return yaml.load(file)


def load_template(path: str) -> Template:
    with open(path, "r", encoding="utf-8") as file:
        content = file.read()
    return Template(content)


def save(path: str, content: str) -> None:
    with open(path, "w", encoding="utf-8") as file:
        file.write(content)


def parse_args(args: list[str]) -> argparse.Namespace:
    parser = argparse.ArgumentParser(
        description="Generate a csproj template and write the result to the output directory.",
    )

    parser.add_argument("template_path", help="template file")
    parser.add_argument("project_name", help="Project name used in the rendered csproj")
    parser.add_argument(
        "--output-dir",
        default=DEFAULT_OUTPUT_DIR,
        help=f"Output directory (default: {DEFAULT_OUTPUT_DIR})",
    )

    return parser.parse_args(args)


def main(args: list[str]):
    # Parse command-line arguments
    parsed_args = parse_args(args)

    # Set variables from arguments
    template_path = parsed_args.template_path
    project_name = parsed_args.project_name
    output_dir = os.path.join(parsed_args.output_dir, project_name)

    # Load template and configuration
    template = load_template(template_path)
    template_variables = load_config()
    template_variables["project_name"] = project_name

    rendered = template.render(**template_variables)

    # Write the rendered content to the output directory
    os.makedirs(output_dir, exist_ok=True)
    output_path = os.path.join(output_dir, f"{project_name}.csproj")

    save(output_path, rendered)

    print(f"Generated {output_path}")


if __name__ == "__main__":
    main(sys.argv[1:])
