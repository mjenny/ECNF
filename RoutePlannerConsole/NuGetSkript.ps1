#Ask for Version
$version = Read-Host "Set assembly version to be published as x.x.x.x"

$pfn = "RoutePlannerLib_JW."
$pfn += $version
$pfn += ".nupkg"

$sfn = "RoutePlannerLib_JW."
$sfn += $version
$sfn += ".symbols.nupkg"


#SetApiKey for the staging environment of nuget
E:\NuGet\NuGet.exe SetApiKey 1d3f3e13-f4b8-4ad8-8819-67272792ed64 -s https://staging.nuget.org/
#Pack the RoutePlannerLib
E:\NuGet\NuGet.exe Pack "E:\FHNW\Semester 7\ecnf 7Ibb\Repo\ECNF\RoutePlannerLib\RoutePlannerLib_JW.csproj"
#Pack the RoutePlannerLib Symbols
E:\NuGet\NuGet.exe Pack "E:\FHNW\Semester 7\ecnf 7Ibb\Repo\ECNF\RoutePlannerLib\RoutePlannerLib_JW.csproj" -Symbols
#Deploy the RoutePlannerLib
E:\NuGet\NuGet.exe Push $pfn -s https://staging.nuget.org/
#Deploy the symbols
E:\NuGet\NuGet.exe Push $sfn -s http://nuget.gw.symbolsource.org/Public/NuGet
#END
Read-Host -Prompt “Press Enter to exit”