#!/bin/bash
echo Retore netstandard 1.1 project...
echo Solution=$1
echo Project=$2
echo Configuration=$3
dotnet restore $2../1.1/MsgPack/project.json
if test $? -ne 0; then
  echo "Failed to restore netstandard1.1"
  return 1 1>/dev/null 2>&1
  exit 1
fi

echo Build netstandard 1.1 project...
dotnet build $2../1.1/MsgPack/project.json -c $3
if test $? -ne 0; then
  echo "Failed to build netstandard1.1"
  return 1 1>/dev/null 2>&1
  exit 1
fi

echo Retore netstandard 1.3 project...
dotnet restore $2../1.3/MsgPack/project.json
if test $? -ne 0; then
  echo "Failed to restore netstandard1.3"
  return 1 1>/dev/null 2>&1
  exit 1
fi

echo Build netstandard 1.1 project...
dotnet build $2../1.3/MsgPack/project.json -c $3
if test $? -ne 0; then
  echo "Failed to build netstandard1.3"
  return 1 1>/dev/null 2>&1
  exit 1
fi

if test "Release" = "$3"; then
  echo Copy release binaries...
  cp -r $2../1.1/bin/Release/* $1bin/
  cp -r $2../1.3/bin/Release/* $1bin/
fi
