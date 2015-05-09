@echo off

set ThisDir=%~dp0

set xsdExe="C:\Program Files\Microsoft SDKs\Windows\v7.0A\Bin\xsd.exe"
set paramsFile=%ThisDir%\params.xml
set outDir=%ThisDir%bin

mkdir %outDir%

echo Creating XSD
%xsdExe% %ThisDir%example.xml /out:"%outDir%"
echo.
echo Creating .cs
%xsdExe% /parameters:"%paramsFile%" /c /out:"%outDir%" /l:CS

pause