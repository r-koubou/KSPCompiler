from __future__ import annotations

from dataclasses import dataclass
from typing import Any, Optional

from ruamel.yaml import YAML
from ruamel.yaml.scalarstring import LiteralScalarString

@dataclass(frozen=True)
class CallbackArgumentModel:
	name: str
	data_type: str
	required_declare: bool
	description: LiteralScalarString = LiteralScalarString("")

	@staticmethod
	def from_dict(data: dict[str, Any]) -> "CallbackArgumentModel":
		description = data.get("Description", "")
		if description is None:
			description = ""

		return CallbackArgumentModel(
			name=str(data["Name"]),
			data_type=str(data["DataType"]),
			required_declare=bool(data["RequiredDeclare"]),
			description=LiteralScalarString(description),
		)

	def to_dict(self) -> dict[str, Any]:
		return {
			"Name": self.name,
			"DataType": self.data_type,
			"RequiredDeclare": self.required_declare,
			"Description": str(self.description),
		}


@dataclass(frozen=True)
class CallbackModel:
	id: str
	name: str
	built_in: bool
	allow_multiple_declaration: bool
	arguments: list[CallbackArgumentModel]
	built_into_version: str = "N/A"
	description: LiteralScalarString = LiteralScalarString("")

	@staticmethod
	def from_dict(data: dict[str, Any]) -> "CallbackModel":
		description = data.get("Description", "")
		if description is None:
			description = ""

		arguments_data = data.get("Arguments", [])
		if not isinstance(arguments_data, list):
			raise ValueError("Arguments must be a list")

		built_into_version = data.get("BuiltIntoVersion", "N/A")
		if built_into_version is None:
			built_into_version = "N/A"

		return CallbackModel(
			id=str(data["Id"]),
			name=str(data["Name"]),
			built_in=bool(data["BuiltIn"]),
			allow_multiple_declaration=bool(data["AllowMultipleDeclaration"]),
			arguments=[CallbackArgumentModel.from_dict(item) for item in arguments_data],
			built_into_version=str(built_into_version),
			description=LiteralScalarString(description),
		)

	def to_dict(self) -> dict[str, Any]:
		return {
			"Id": self.id,
			"Name": self.name,
			"BuiltIn": self.built_in,
			"AllowMultipleDeclaration": self.allow_multiple_declaration,
			"Description": str(self.description),
			"BuiltIntoVersion": self.built_into_version,
			"Arguments": [arg.to_dict() for arg in self.arguments],
		}


@dataclass(frozen=True)
class CallbackModelRoot:
	format_version: str
	data: list[CallbackModel]

	@staticmethod
	def from_dict(raw: dict[str, Any]) -> "CallbackModelRoot":
		data_field = raw["Data"]
		return CallbackModelRoot(
			format_version=str(raw["FormatVersion"]),
			data=[CallbackModel.from_dict(item) for item in data_field],
		)

	@staticmethod
	def from_yaml(path: str) -> "CallbackModelRoot":
		with open(path, "r", encoding="utf-8") as f:
			loaded: Optional[dict[str, Any]] = YAML().load(f)

		if not isinstance(loaded, dict):
			raise ValueError("YAML root must be a mapping")

		return CallbackModelRoot.from_dict(loaded)

	def to_dict(self) -> dict[str, Any]:
		return {
			"FormatVersion": self.format_version,
			"Data": [item.to_dict() for item in self.data],
		}
