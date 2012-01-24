@echo Off
set config=%1
if "%config%" == "" (
   set config=debug
)

:: compile the code
echo ===========Building WP7 client===========
%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild src/Samurai.Client.Wp7/Samurai.Client.Wp7.sln /p:Configuration="%config%" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:false

echo ===========Building Server===========
%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild src/SamuraiServer/SamuraiServer.sln /p:Configuration="%config%" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:false

echo ===========Tests===========
:: remove all obj folder contents
for /D %%f in (".\src\SamuraiServer\SamuraiServer.Tests\*") do @(
del /S /Q "%%f\obj\*"
)

:: find all test files and run them
for /R %%F in (*Tests.dll) do (
.\tools\xunit\xunit.console.clr4.exe %%F
)