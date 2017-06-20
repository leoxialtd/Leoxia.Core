# Leoxia.Core

[![Build status](https://ci.appveyor.com/api/projects/status/2xrjylsvbxfotsoo?svg=true)](https://ci.appveyor.com/project/leoxialtd/leoxia-core)

[![NuGet version](https://badge.fury.io/nu/Leoxia.Core.svg)](https://badge.fury.io/nu/Leoxia.Core)

Leoxia.Core or Lx.Core regroups all tranversal and low level libraries developped by Leoxia.

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
