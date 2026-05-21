@echo off
chcp 65001 >nul
echo.
echo  ╔═══════════════════════════════════════════╗
echo  ║           Jbot — Сборка проекта           ║
echo  ╚═══════════════════════════════════════════╝
echo.

:: ── Проверка .NET SDK ──
where dotnet >nul 2>&1
if %errorlevel% neq 0 (
    echo  [ОШИБКА] .NET SDK не найден!
    echo  Скачайте .NET 10 SDK: https://dotnet.microsoft.com/download
    echo.
    pause
    exit /b 1
)

echo  [1/3] Проверка .NET SDK...
for /f "tokens=*" %%v in ('dotnet --version') do echo        Версия: %%v

:: ── Проверка MAUI workload ──
echo  [2/3] Проверка MAUI workload...
dotnet workload list 2>nul | findstr /i "maui" >nul
if %errorlevel% neq 0 (
    echo        MAUI workload не установлен. Устанавливаю...
    echo.
    dotnet workload install maui
    if %errorlevel% neq 0 (
        echo  [ОШИБКА] Не удалось установить MAUI workload
        pause
        exit /b 1
    )
)
echo        MAUI workload OK

:: ── Проверка .env ──
if not exist "%~dp0.env" (
    echo.
    echo  [!] Файл .env не найден!
    echo      Копирую .env.example в .env...
    copy "%~dp0.env.example" "%~dp0.env" >nul
    echo.
    echo  ╔═══════════════════════════════════════════╗
    echo  ║  Откройте .env и заполните ваши данные:   ║
    echo  ║                                           ║
    echo  ║  APPLICATION_KEY=ваш_ключ                 ║
    echo  ║  USERNAME=ваш_логин                       ║
    echo  ║  PASSWORD=ваш_пароль                      ║
    echo  ╚═══════════════════════════════════════════╝
    echo.
    echo  После заполнения запустите build.bat снова.
    pause
    exit /b 0
)

:: ── Сборка ──
echo  [3/3] Сборка проекта...
echo.
dotnet build "%~dp0Jbot\Jbot.csproj" -f net10.0-windows10.0.19041.0 -c Release --nologo
if %errorlevel% neq 0 (
    echo.
    echo  [ОШИБКА] Сборка завершилась с ошибками.
    pause
    exit /b 1
)

echo.
echo  ╔═══════════════════════════════════════════╗
echo  ║       Сборка завершена успешно! ✓         ║
echo  ║                                           ║
echo  ║   Запустите run.bat для старта Jbot       ║
echo  ╚═══════════════════════════════════════════╝
echo.
pause
