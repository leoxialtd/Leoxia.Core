
ForEach ($file in Get-ChildItem test)
{
	$project = ".\test\" + $file.Name + "\" + $file.Name + ".csproj"
	echo "Testing " + $project
	dotnet test $project --configuration release 
	if ($LASTEXITCODE -ne 0)
	{
		exit $LASTEXITCODE
	}
}
