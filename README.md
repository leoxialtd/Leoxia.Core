# Leoxia.Core

All general purposes and low level libraries used by Leoxia organization.

[![.NET Core](https://img.shields.io/badge/Build_For-.NetCore-5C2D91.svg)](https://www.microsoft.com/net/core#windowsvs2017)

[![.NET Standard](https://img.shields.io/badge/Build_For-.NetStandard-0073AE.svg)](https://docs.microsoft.com/en-us/dotnet/standard/net-standard)

## Build status

OS  | Compiler | Status 
-------- | :------------ | :------------ 
Windows | Visual Studio 2017 | [![Build status](https://ci.appveyor.com/api/projects/status/2xrjylsvbxfotsoo?svg=true)](https://ci.appveyor.com/project/leoxialtd/leoxia-core)

## Packages

Package  | NuGet 
-------- | :------------ 
[Leoxia.Collections](https://github.com/leoxialtd/Leoxia.Core/tree/master/src/Leoxia.Collections) | [![NuGet version](https://badge.fury.io/nu/Leoxia.Collections.svg)](https://www.nuget.org/packages/Leoxia.Collections/)
[Leoxia.Collections.Concurrent](https://github.com/leoxialtd/Leoxia.Core/tree/master/src/Leoxia.Collections.Concurrent) | [![NuGet version](https://badge.fury.io/nu/Leoxia.Collections.Concurrent.svg)](https://www.nuget.org/packages/Leoxia.Collections.Concurrent/)
[Leoxia.Common](https://github.com/leoxialtd/Leoxia.Core/tree/master/src/Leoxia.Common) | [![NuGet version](https://badge.fury.io/nu/Leoxia.Common.svg)](https://www.nuget.org/packages/Leoxia.Common/)
[Leoxia.Configuration](https://github.com/leoxialtd/Leoxia.Core/tree/master/src/Leoxia.Configuration) | [![NuGet version](https://badge.fury.io/nu/Leoxia.Configuration.svg)](https://www.nuget.org/packages/Leoxia.Configuration/)
[Leoxia.Graphs](https://github.com/leoxialtd/Leoxia.Core/tree/master/src/Leoxia.Graphs) | [![NuGet version](https://badge.fury.io/nu/Leoxia.Graphs.svg)](https://www.nuget.org/packages/Leoxia.Graphs/)
[Leoxia.IO](https://github.com/leoxialtd/Leoxia.Core/tree/master/src/Leoxia.IO) | [![NuGet version](https://badge.fury.io/nu/Leoxia.IO.svg)](https://www.nuget.org/packages/Leoxia.IO/)
[Leoxia.Log](https://github.com/leoxialtd/Leoxia.Core/tree/master/src/Leoxia.Log) | [![NuGet version](https://badge.fury.io/nu/Leoxia.Log.svg)](https://www.nuget.org/packages/Leoxia.Log/)
[Leoxia.Network](https://github.com/leoxialtd/Leoxia.Core/tree/master/src/Leoxia.Network) | [![NuGet version](https://badge.fury.io/nu/Leoxia.Network.svg)](https://www.nuget.org/packages/Leoxia.Network/)
[Leoxia.Reflection](https://github.com/leoxialtd/Leoxia.Core/tree/master/src/Leoxia.Reflection) | [![NuGet version](https://badge.fury.io/nu/Leoxia.Reflection.svg)](https://www.nuget.org/packages/Leoxia.Reflection/)
[Leoxia.Security](https://github.com/leoxialtd/Leoxia.Core/tree/master/src/Leoxia.Security) | [![NuGet version](https://badge.fury.io/nu/Leoxia.Security.svg)](https://www.nuget.org/packages/Leoxia.Security/)
[Leoxia.Serialization.Json](https://github.com/leoxialtd/Leoxia.Core/tree/master/src/Leoxia.Serialization.Json) | [![NuGet version](https://badge.fury.io/nu/Leoxia.Serialization.Json.svg)](https://www.nuget.org/packages/Leoxia.Serialization.Json/)
[Leoxia.Testing.Assertions](https://github.com/leoxialtd/Leoxia.Core/tree/master/src/Leoxia.Testing.Assertions) | [![NuGet version](https://badge.fury.io/nu/Leoxia.Testing.Assertions.svg)](https://www.nuget.org/packages/Leoxia.Testing.Assertions/)
[Leoxia.Testing.Checkers](https://github.com/leoxialtd/Leoxia.Core/tree/master/src/Leoxia.Testing.Checkers) | [![NuGet version](https://badge.fury.io/nu/Leoxia.Testing.Checkers.svg)](https://www.nuget.org/packages/Leoxia.Testing.Checkers/)
[Leoxia.Testing.Mocks](https://github.com/leoxialtd/Leoxia.Core/tree/master/src/Leoxia.Testing.Mocks) | [![NuGet version](https://badge.fury.io/nu/Leoxia.Testing.Mocks.svg)](https://www.nuget.org/packages/Leoxia.Testing.Mocks/)
[Leoxia.Testing.Reflection](https://github.com/leoxialtd/Leoxia.Core/tree/master/src/Leoxia.Testing.Reflection) | [![NuGet version](https://badge.fury.io/nu/Leoxia.Testing.Reflection.svg)](https://www.nuget.org/packages/Leoxia.Testing.Reflection/)
[Leoxia.Text.Extensions](https://github.com/leoxialtd/Leoxia.Core/tree/master/src/Leoxia.Text.Extensions) | [![NuGet version](https://badge.fury.io/nu/Leoxia.Text.Extensions.svg)](https://www.nuget.org/packages/Leoxia.Text.Extensions/)
[Leoxia.Threading](https://github.com/leoxialtd/Leoxia.Core/tree/master/src/Leoxia.Threading) | [![NuGet version](https://badge.fury.io/nu/Leoxia.Threading.svg)](https://www.nuget.org/packages/Leoxia.Threading/)

# Requirements

To be part of Lx.Core, a library must:

- Contains classes easy to reuse in any kind of context.
- Contains classes with an intended unique implementation: That must remain 
unique now and ever in the company.
- Contains only the official implementations of general purpose classes.
- Be a .NetLibrary lesser or equal to 1.5 
- Be tested by the relevant test library.
- Be named Leoxia.[Namespace] with Namespace existing in System or being a 
similar general-use name. Refer to namespace rules.

Lx is the product name but it is not included in the assembly/package/namespace 
names because there is no reason to have another version of the library for 
another product.
