from __future__ import annotations

from dataclasses import dataclass
from typing import Any, Optional

from ruamel.yaml import YAML
from ruamel.yaml.scalarstring import LiteralScalarString

@dataclass(frozen=True)
class VariableModel:
	id: str
	name: str
	built_in: bool
	built_into_version: str
	description: LiteralScalarString = LiteralScalarString("")

	@staticmethod
	def from_dict(data: dict[str, Any]) -> "VariableModel":
		description = data.get("Description", "")
		if description is None:
			description = ""

		return VariableModel(
			id=str(data["Id"]),
			name=str(data["Name"]),
			built_in=bool(data["BuiltIn"]),
			built_into_version=str(data["BuiltIntoVersion"]),
			description=LiteralScalarString(description),
		)

	def to_dict(self) -> dict[str, Any]:
		return {
			"Id": self.id,
			"Name": self.name,
			"BuiltIn": self.built_in,
			"Description": str(self.description),
			"BuiltIntoVersion": self.built_into_version,
		}


@dataclass(frozen=True)
class VariableRootModel:
	format_version: str
	data: list[VariableModel]

	@staticmethod
	def from_dict(raw: dict[str, Any]) -> "VariableRootModel":
		data_field = raw["Data"]
		return VariableRootModel(
			format_version=str(raw["FormatVersion"]),
			data=[VariableModel.from_dict(item) for item in data_field],
		)

	@staticmethod
	def from_yaml(path: str) -> "VariableRootModel":
		with open(path, "r", encoding="utf-8") as f:
			loaded: Optional[dict[str, Any]] = YAML().load(f)

		if not isinstance(loaded, dict):
			raise ValueError("YAML root must be a mapping")

		return VariableRootModel.from_dict(loaded)

	def to_dict(self) -> dict[str, Any]:
		return {
			"FormatVersion": self.format_version,
			"Data": [item.to_dict() for item in self.data],
		}
