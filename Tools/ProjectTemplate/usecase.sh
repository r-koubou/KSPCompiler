#!/bin/bash

uv run python ./gen_project.py module UseCase.$1
uv run python ./gen_project.py module Interactor.$1
