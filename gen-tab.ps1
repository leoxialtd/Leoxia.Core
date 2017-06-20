ForEach ($file in Get-ChildItem src)
{
	echo "[$file](https://github.com/leoxialtd/Leoxia.Core/tree/master/src/$file) | [![NuGet version](https://badge.fury.io/nu/$file.svg)](https://www.nuget.org/packages/$file/)"
}