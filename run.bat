@echo off
chcp 65001 >nul
echo.
echo  ╔═══════════════════════════════════════════╗
echo  ║           Jbot — Запуск приложения        ║
echo  ╚═══════════════════════════════════════════╝
echo.

:: ── Проверка .env ──
if not exist "%~dp0.env" (
    echo  [ОШИБКА] Файл .env не найден!
    echo  Сначала запустите build.bat или создайте .env из .env.example
    echo.
    pause
    exit /b 1
)

:: ── Проверка сборки ──
set "EXE_PATH=%~dp0Jbot\bin\Release\net10.0-windows10.0.19041.0\Jbot.exe"
if not exist "%EXE_PATH%" (
    echo  [!] Приложение ещё не собрано. Запускаю сборку...
    echo.
    call "%~dp0build.bat"
    if %errorlevel% neq 0 exit /b 1
)

:: ── Запуск ──
echo  Запускаю Jbot...
echo.
start "" "%EXE_PATH%"
