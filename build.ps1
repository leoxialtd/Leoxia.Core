# restore and builds all projects as release.
echo "DotNet Version:"
dotnet --version

$solutionName = "Leoxia.Core.sln"

dotnet restore $solutionName

dotnet build $solutionName --configuration release 

# Note that we don't build packages here because 
# - we should build only once each package
# - we should not build test package
# - we should not push package on Nuget for each build (only done for official release).