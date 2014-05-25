#!/bin/bash
# Build Unity package

# Note that this script only supports MacOS X.

UNITY=/Applications/Unity/Unity.app/Contents/MacOS/Unity 

if [ ! -f $UNITY ];
then
    echo "Unity is not found."
    exit 1
fi

MPU=./tools/mpu/bin/mpu.exe

if [ ! -f $MPU ];
then
    xbuild ./MsgPack.compats.sln /p:Configuration=Release
fi

PROJECTPATH=$(cd $(dirname "./src/MsgPack.Unity") && pwd)/$(basename "./src/MsgPack.Unity")/
PACKAGENAME=$(cd $(dirname "./MsgPack.unitypackage") && pwd)/$(basename "./MsgPack.unitypackage")

rm -rdf ./src/MsgPack.Unity/Assets/MsgPack/*
rm -f ./MsgPack.unitypackage
mono $MPU -l -p ./src/MsgPack.Xamarin.iOS/MsgPack.Xamarin.iOS.csproj -o ./src/MsgPack.Unity/Assets/MsgPack/ -w
$UNITY -quit -projectPath PROJECTPATH -exportPackage Assets/MsgPack PACKAGENAME
