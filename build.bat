@echo off

color 2

REM Build the plugin
dotnet build

REM Check the exit code of the build command
if %errorlevel% equ 0 (
    echo Build succeeded.
) else (
    echo Build failed with error code %errorlevel%.
    exit /b %errorlevel%
)

set game_path=D:/Games/modded/ULTRAKILL
set project_name=ultrafab

color 9

REM Copy the DLL and PDB files to the plugin directory
copy "bin\Debug\net4.7.2\%project_name%.dll" "%game_path%/BepInEx/plugins"
copy "bin\Debug\net4.7.2\%project_name%.pdb" "%game_path%/BepInEx/plugins"

color 8

REM Ask the user if they want to open the game
set /p open_game=Do you want to open the game? (Y/n) 
if not defined open_game set open_game=Y

REM If the user answers "Y" or "y", open the game, otherwise exit the script
if /i "%open_game%"=="Y" (
    start "" "%game_path%/ULTRAKILL.exe"
) else (
    exit /b
)

color

@echo on
