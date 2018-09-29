#!/bin/bash
set -e
cd `dirname $0`

rm -rf ../artifacts/Documentation
mkdir -p ../artifacts/Documentation

# Download tag files for external references
curl -o nanobyte-common.tag https://common.nanobyte.de/nanobyte-common.tag

doxygen

cp CNAME ..\artifacts\Documentation\
