@echo off
call PackXamarin.bat
pause
call Pack.bat
pause
:: Rename .latest manually and upload it with .nuget\nuget.exe
