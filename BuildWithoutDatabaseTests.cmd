@echo off
lib\nant\bin\nant.exe -D:dbtest.enabled=false -D:project.configuration=release %*

pause