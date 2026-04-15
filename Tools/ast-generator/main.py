import argparse
import sys
import os
import os.path

from ruamel.yaml import YAML
from jinja2 import Template

THIS_SCRIPT_DIR = os.path.dirname(os.path.abspath(__file__))

DEFINITIONS_DIR = os.path.join(THIS_SCRIPT_DIR, "definitions")
DEFAULT_OUTPUT_DIR = os.path.join(THIS_SCRIPT_DIR, "out")
DEFAULT_AST_CLASS_PREFIX = "Ast"

yaml = YAML()


def load_definition(file_path: str) -> dict:
    with open(file_path, "r") as f:
        definition = yaml.load(f)
    return definition


def load_definitions() -> list[dict]:
    definitions = []
    for filename in os.listdir(DEFINITIONS_DIR):
        if not filename.endswith(".yaml") and not filename.endswith(".yml"):
            continue

        definition = load_definition(os.path.join(DEFINITIONS_DIR, filename))
        definitions.append(definition)

    return definitions


def make_class_name(ast_class_prefix: str, class_name: str) -> str:
    return ast_class_prefix + class_name


def make_class_file_name(class_name: str, definition: dict) -> str:
    return class_name + definition["suffix"]


def render_template(definition: dict, ast_class_prefix: str, output_base_dir: str):
    namespace = definition["namespace"]
    template_path = definition["template"]
    output_dir = output_base_dir

    if "output_dir" in definition:
        output_dir = os.path.join(output_base_dir, definition["output_dir"])

    for clazz in definition["classes"]:
        class_name = make_class_name(ast_class_prefix, clazz["name"])
        description = clazz["description"]

        with open(template_path, "r") as f:
            template = Template(f.read())
            rendered = template.render(
                namespace=namespace,
                class_name=class_name,
                name=class_name,
                description=description,
            )

            output_path = os.path.join(
                output_dir, make_class_file_name(class_name, definition)
            )
            os.makedirs(os.path.dirname(output_path), exist_ok=True)
            with open(output_path, "w") as out_f:
                out_f.write(rendered)


def render_visitor(ast_class_prefix: str, output_base_dir: str, class_names: list[str]):
    definition = load_definition(os.path.join(DEFINITIONS_DIR, "visitor.yaml"))
    namespace = definition["namespace"]
    template_path = definition["template"]
    output_dir = output_base_dir
    suffix = definition["suffix"]

    if "output_dir" in definition:
        output_dir = os.path.join(output_base_dir, definition["output_dir"])

    with open(template_path, "r") as f:
        template = Template(f.read())
        rendered = template.render(
            namespace=namespace,
            ast_class_prefix=ast_class_prefix,
            class_names=class_names,
        )

        output_path = os.path.join(output_dir, f"IAstVisitor{suffix}")
        os.makedirs(os.path.dirname(output_path), exist_ok=True)
        with open(output_path, "w") as out_f:
            out_f.write(rendered)


def render_enum(names: list[str], output_base_dir: str):
    definition = load_definition(os.path.join(DEFINITIONS_DIR, "ast_node_id.yaml"))
    namespace = definition["namespace"]
    template_path = definition["template"]
    output_dir = output_base_dir
    suffix = definition["suffix"]

    if "output_dir" in definition:
        output_dir = os.path.join(output_base_dir, definition["output_dir"])

    with open(template_path, "r") as f:
        template = Template(f.read())
        rendered = template.render(
            namespace=namespace,
            names=names,
        )

        output_path = os.path.join(output_dir, f"AstNodeId{suffix}")
        os.makedirs(os.path.dirname(output_path), exist_ok=True)
        with open(output_path, "w") as out_f:
            out_f.write(rendered)


def parse_args(args: list[str]) -> argparse.Namespace:
    parser = argparse.ArgumentParser(
        description="Generate a AST node class template and Visitor interface",
    )

    parser.add_argument(
        "--output-base-dir",
        default=DEFAULT_OUTPUT_DIR,
        help=f"Output base directory (default: {DEFAULT_OUTPUT_DIR})",
    )

    parser.add_argument(
        "--ast-class-prefix",
        default=DEFAULT_AST_CLASS_PREFIX,
        help=f"Prefix for AST node classes (default: {DEFAULT_AST_CLASS_PREFIX})",
    )

    return parser.parse_args(args)


def main(args: list[str]):
    # Parse command-line arguments
    parsed_args = parse_args(args)

    # Set variables from arguments
    output_base_dir = parsed_args.output_base_dir
    ast_class_prefix = parsed_args.ast_class_prefix

    # Load definitions
    definition_base = load_definition(os.path.join(DEFINITIONS_DIR, "base.yaml"))
    definition_block = load_definition(os.path.join(DEFINITIONS_DIR, "block.yaml"))
    definition_expression = load_definition(
        os.path.join(DEFINITIONS_DIR, "expression.yaml")
    )
    definition_statement = load_definition(
        os.path.join(DEFINITIONS_DIR, "statements.yaml")
    )

    definitions = [
        definition_base,
        definition_block,
        definition_expression,
        definition_statement,
    ]

    # Render templates
    node_names = []
    node_class_names = []
    for x in definitions:
        render_template(x, ast_class_prefix, output_base_dir)

        for clazz in x["classes"]:
            node_names.append(clazz["name"])
            node_class_names.append(make_class_name(ast_class_prefix, clazz["name"]))

    # Render Visitor interface
    render_visitor(ast_class_prefix, output_base_dir, node_class_names)

    # Render AstNodeId enum
    render_enum(node_names, output_base_dir)


if __name__ == "__main__":
    main(sys.argv[1:])
