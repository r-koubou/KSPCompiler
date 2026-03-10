from __future__ import annotations

from dataclasses import dataclass
from typing import Any, Optional

import yaml


@dataclass(frozen=True)
class InitializerArgumentModel:
	name: str
	data_type: str
	description: str = ""

	@staticmethod
	def from_dict(data: dict[str, Any]) -> "InitializerArgumentModel":
		description = data.get("Description", "")
		if description is None:
			description = ""

		return InitializerArgumentModel(
			name=str(data["Name"]),
			data_type=str(data["DataType"]),
			description=str(description),
		)

	def to_dict(self) -> dict[str, Any]:
		return {
			"Name": self.name,
			"DataType": self.data_type,
			"Description": self.description,
		}


@dataclass(frozen=True)
class UITypeModel:
	id: str
	name: str
	built_in: bool
	variable_type: str
	require_initializer: bool
	initializer_arguments: list[InitializerArgumentModel]
	built_into_version: str = "N/A"
	description: str = ""

	@staticmethod
	def from_dict(data: dict[str, Any]) -> "UITypeModel":
		description = data.get("Description", "")
		if description is None:
			description = ""

		built_into_version = data.get("BuiltIntoVersion", "N/A")
		if built_into_version is None:
			built_into_version = "N/A"

		initializer_arguments_data = data.get("InitializerArguments", [])
		if not isinstance(initializer_arguments_data, list):
			raise ValueError("InitializerArguments must be a list")

		return UITypeModel(
			id=str(data["Id"]),
			name=str(data["Name"]),
			built_in=bool(data["BuiltIn"]),
			variable_type=str(data["VariableType"]),
			require_initializer=bool(data["RequireInitializer"]),
			initializer_arguments=[
				InitializerArgumentModel.from_dict(item) for item in initializer_arguments_data
			],
			built_into_version=str(built_into_version),
			description=str(description),
		)

	def to_dict(self) -> dict[str, Any]:
		return {
			"Id": self.id,
			"Name": self.name,
			"BuiltIn": self.built_in,
			"VariableType": self.variable_type,
			"Description": self.description,
			"BuiltIntoVersion": self.built_into_version,
			"RequireInitializer": self.require_initializer,
			"InitializerArguments": [arg.to_dict() for arg in self.initializer_arguments],
		}


@dataclass(frozen=True)
class UITypeModelRoot:
	format_version: str
	data: list[UITypeModel]

	@staticmethod
	def from_dict(raw: dict[str, Any]) -> "UITypeModelRoot":
		data_field = raw["Data"]
		return UITypeModelRoot(
			format_version=str(raw["FormatVersion"]),
			data=[UITypeModel.from_dict(item) for item in data_field],
		)

	@staticmethod
	def from_yaml(path: str) -> "UITypeModelRoot":
		with open(path, "r", encoding="utf-8") as f:
			loaded: Optional[dict[str, Any]] = yaml.safe_load(f)

		if not isinstance(loaded, dict):
			raise ValueError("YAML root must be a mapping")

		return UITypeModelRoot.from_dict(loaded)

	def to_dict(self) -> dict[str, Any]:
		return {
			"FormatVersion": self.format_version,
			"Data": [item.to_dict() for item in self.data],
		}
