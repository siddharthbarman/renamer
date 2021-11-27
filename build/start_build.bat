ECHO OFF

IF %1.==. (
   ECHO Platform not specified. E.g. start_build "Any CPU" release
   EXIT /B
)

IF %2.==. (
   ECHO Configuration not specified. E.g. start_build "Any CPU" release
   EXIT /B
)

SET NSIS_PATH=C:\Program Files (x86)\NSIS
SET HELP_COMPILER=C:\Program Files (x86)\HTML Help Workshop
SET VS_PATH=C:\Program Files\Microsoft Visual Studio\2022\Community\Msbuild\Current\Bin

BUILD.BAT %1 %2

