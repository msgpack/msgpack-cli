#!/bin/bash
echo Retore netstandard 1.1 project...
echo Solution=$1
echo Configuration=$2
dotnet restore $1src/MsgPack.CoreClr/project.json
if test $? -ne 0; then
  echo "Failed to restore netstandard1.1/1.3"
  return 1 1>/dev/null 2>&1
  exit 1
fi

echo Build netstandard 1.1/1.3 project...
dotnet build $1src/MsgPack.CoreClr/project.json -c $2
if test $? -ne 0; then
  echo "Failed to build netstandard1.1/1.3"
  return 1 1>/dev/null 2>&1
  exit 1
fi
