#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyExtensions.cs" company="Leoxia Ltd">
//    Copyright (c) 2017 Leoxia Ltd
// </copyright>
// 
// .NET Software Development
// https://www.leoxia.com
// Build. Tomorrow. Together
// 
// MIT License
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//  --------------------------------------------------------------------------------------------------------------------

#endregion

#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Leoxia.Abstractions;
using Leoxia.Implementations;

#endregion

namespace Leoxia.Testing.Reflection
{
    public static class AssemblyExtensions
    {
        private static readonly IAssembly _system;
        private static readonly string _systemPath;

        static AssemblyExtensions()
        {
            _system = typeof(string).GetTypeInfo().Assembly.Wrap();
            _systemPath = Path.GetDirectoryName(_system.Location);
        }

        /// <summary>
        ///     Gets all types declared in given assembly and in all the assemblies referenced by the given assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>array of types contains in assembly and its references</returns>
        public static Type[] GetAllReferencedTypes(this IAssembly assembly)
        {
            var types = new List<Type>();
            var assemblies = GetAllReferencedAssemblies(assembly, null, null);
            foreach (var subAssembly in assemblies)
            {
                types.AddRange(subAssembly.GetTypes());
            }
            return types.ToArray();
        }


        public static IAssembly[] GetAllReferencedAssemblies(this IAssembly assembly, string pattern,
            string excludePattern)
        {
            var assemblies = new Dictionary<string, IAssembly>();
            var marked = new HashSet<string>();
            return GetAllReferencedAssemblies(assembly, pattern, excludePattern, marked, assemblies);
        }

        private static IAssembly[] GetAllReferencedAssemblies(IAssembly assembly, string pattern, string excludePattern,
            HashSet<string> marked, Dictionary<string, IAssembly> assemblies)
        {
            var name = assembly.GetName().Name;
            marked.Add(name);
            if (!assembly.IsFrameworkAssembly())
            {
                if (CheckPattern(assembly, pattern, excludePattern))
                {
                    if (!assemblies.ContainsKey(name))
                    {
                        assemblies.Add(name, assembly);
                    }
                }
                foreach (var assemblyName in assembly.GetReferencedAssemblies())
                {
                    if (assemblyName.FullName.Contains(pattern))
                    {
                        if (!assemblyName.FullName.Contains("Unit"))
                        {
                            if (!marked.Contains(assemblyName.Name))
                            {
                                var referenced = Assembly.Load(assemblyName).Wrap();
                                GetAllReferencedAssemblies(referenced, pattern, excludePattern, marked, assemblies);
                            }
                        }
                    }
                }
            }
            return assemblies.Values.ToArray();
        }

        private static bool CheckPattern(IAssembly assembly, string pattern, string excludePattern)
        {
            return assembly.FullName.Contains(pattern) && !assembly.FullName.Contains(excludePattern);
        }

        public static bool IsFrameworkAssembly(this IAssembly assembly)
        {
            if (!string.IsNullOrEmpty(assembly.Location))
            {
                return Path.GetDirectoryName(assembly.Location) == _systemPath;
            }
            return false;
        }
    }
}