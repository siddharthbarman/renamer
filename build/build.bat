ECHO OFF

REM ** Expected environment variables **
REM HELP_COMPILER - folder where Microsoft help compiler is located
REM VS_PATH       - folder where Visual Studio is located
REM NSIS_PATH     - folder where NSIS is located
REM PLATFORM      - can be either x86 or x64

SET PLATFORM=%1
SET CONFIG=%2

IF NOT DEFINED CONFIG (
   SET CONFIG=Release
)

IF DEFINED PLATFORM (
   ECHO Compiling for %PLATFORM% %CONFIG%
) ELSE (
   ECHO No platform specified "Any CPU" or x86 or x64, exiting.
   GOTO:EOF
)

REM Setup up required paths

ECHO OFF
rem SET PATH=%PATH%;%HELP_COMPILER%
rem SET PATH=%PATH%;%NSIS_PATH%

REM Clean Setup Files
cd ..\install
del /q files\*.*
del /q renamer.zip
cd ..\build

REM Copy Help Files
cd copy ..\help\readme.txt .\files\

REM Build Source
cd ..\source\renamer
"%VS_PATH%\msbuild" /p:Configuration=%CONFIG%;Platform=%PLATFORM% /t:restore renamer.sln
"%VS_PATH%\msbuild" /p:Configuration=%CONFIG%;Platform=%PLATFORM% renamer.sln

copy app\bin\%CONFIG%\netcoreapp3.1\engine.dll ..\..\install\files
copy app\bin\%CONFIG%\netcoreapp3.1\Newtonsoft.Json.dll ..\..\install\files
copy app\bin\%CONFIG%\netcoreapp3.1\renamer.dll ..\..\install\files
copy app\bin\%CONFIG%\netcoreapp3.1\renamer.exe ..\..\install\files
copy app\bin\%CONFIG%\netcoreapp3.1\renamer.runtimeconfig.json ..\..\install\files

cd ..\..\build

REM Start Building Setup
cd ..\install\files
zip ..\renamer.zip *.*
cd ..\..\build

REM "renamer.zip has been created in <install> folder."





