#!/bin/bash

pushd `dirname $0` > /dev/null
this_dir=`pwd`
popd > /dev/null

uv run python $this_dir/simple_codegen.py "${@}"