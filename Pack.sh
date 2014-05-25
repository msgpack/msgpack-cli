#!/bin/bash

xbuild ./MsgPack.Xamarin.iOS.sln /p:Configuration=Release
xbuild ./MsgPack.Xamarin.Android.sln /p:Configuration=Release

echo Copy ./bin content to Windows machine and execute Pack.ps1 in it.

