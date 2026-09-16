@echo off
setlocal

set UNITY_EXE=C:\Program Files\Unity 6000.6.0f1\Editor\Unity.exe
set PROJECT_DIR=C:\Dev\SchoolYardArena
set LOG_FILE=%PROJECT_DIR%\Builds\unity-build.log

if not exist "%UNITY_EXE%" (
  echo Unity Editor not found at "%UNITY_EXE%"
  exit /b 1
)

if not exist "%PROJECT_DIR%\Builds" mkdir "%PROJECT_DIR%\Builds"

"%UNITY_EXE%" ^
  -batchmode ^
  -quit ^
  -projectPath "%PROJECT_DIR%" ^
  -executeMethod SchoolYardArea.Editor.PrototypeSceneBuilder.CreatePrototypeScene ^
  -logFile "%PROJECT_DIR%\Builds\unity-scene-create.log"

if errorlevel 1 exit /b %errorlevel%

"%UNITY_EXE%" ^
  -batchmode ^
  -quit ^
  -projectPath "%PROJECT_DIR%" ^
  -executeMethod SchoolYardArea.Editor.AndroidBuild.BuildDebugApk ^
  -logFile "%LOG_FILE%"

exit /b %errorlevel%
