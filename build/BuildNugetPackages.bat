@echo on

SET ThisDir=%~dp0
SET NugetExe=%ThisDir%..\src\packages\NuGet.CommandLine.2.8.5\tools\nuget.exe

SET EnvCryptNuSpec=%ThisDir%..\src\EnvCrypt.Core.nuspec
SET EnvCryptSymbolsNuSpec=%ThisDir%..\src\EnvCrypt.Core.symbols.nuspec

SET OutDir=%ThisDir%\..\out

mkdir %OutDir%

%NugetExe% pack %EnvCryptNuSpec% -symbols -OutputDirectory %OutDir%
rem %NugetExe% pack %EnvCryptSymbolsNuSpec% -OutputDirectory %OutDir%