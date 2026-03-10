from __future__ import annotations

from dataclasses import dataclass
from typing import Any, Optional

import yaml


@dataclass(frozen=True)
class ArgumentModel:
	name: str
	data_type: str
	description: str = ""

	@staticmethod
	def from_dict(data: dict[str, Any]) -> "ArgumentModel":
		description = data.get("Description", "")
		if description is None:
			description = ""

		return ArgumentModel(
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
class CommandModel:
	id: str
	name: str
	built_in: bool
	built_into_version: str
	return_type: str
	arguments: list[ArgumentModel]
	description: str = ""

	@staticmethod
	def from_dict(data: dict[str, Any]) -> "CommandModel":
		description = data.get("Description", "")
		if description is None:
			description = ""

		arguments_data = data.get("Arguments", [])
		if not isinstance(arguments_data, list):
			raise ValueError("Arguments must be a list")

		return CommandModel(
			id=str(data["Id"]),
			name=str(data["Name"]),
			built_in=bool(data["BuiltIn"]),
			built_into_version=str(data["BuiltIntoVersion"]),
			return_type=str(data["ReturnType"]),
			arguments=[ArgumentModel.from_dict(item) for item in arguments_data],
			description=str(description),
		)

	def to_dict(self) -> dict[str, Any]:
		return {
			"Id": self.id,
			"Name": self.name,
			"BuiltIn": self.built_in,
			"Description": self.description,
			"BuiltIntoVersion": self.built_into_version,
			"ReturnType": self.return_type,
			"Arguments": [arg.to_dict() for arg in self.arguments],
		}


@dataclass(frozen=True)
class CommandRootModel:
	format_version: str
	data: list[CommandModel]

	@staticmethod
	def from_dict(raw: dict[str, Any]) -> "CommandRootModel":
		data_field = raw["Data"]
		return CommandRootModel(
			format_version=str(raw["FormatVersion"]),
			data=[CommandModel.from_dict(item) for item in data_field],
		)

	@staticmethod
	def from_yaml(path: str) -> "CommandRootModel":
		with open(path, "r", encoding="utf-8") as f:
			loaded: Optional[dict[str, Any]] = yaml.safe_load(f)

		if not isinstance(loaded, dict):
			raise ValueError("YAML root must be a mapping")

		return CommandRootModel.from_dict(loaded)

	def to_dict(self) -> dict[str, Any]:
		return {
			"FormatVersion": self.format_version,
			"Data": [item.to_dict() for item in self.data],
		}
