#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultiChecker.cs" company="Leoxia Ltd">
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
using System.Reflection;
using Leoxia.Abstractions;
using Leoxia.Testing.Reflection;

#endregion

namespace Leoxia.Testing.Checkers
{
    /// <summary>
    ///     Checks all type implementing a given interface with a given checker
    /// </summary>
    public class MultiChecker
    {
        private readonly Type concreteGenericType;
        private readonly List<Type> typesToCheck = new List<Type>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="MultiChecker" /> class.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="checkerType">Type of the checker.</param>
        public MultiChecker(IAssembly assembly, Type interfaceType, Type checkerType)
        {
            concreteGenericType = checkerType;
            foreach (var type in assembly.GetAllReferencedTypes())
            {
                Type specializedGeneric;
                if (interfaceType.GetTypeInfo().ContainsGenericParameters)
                {
                    specializedGeneric = interfaceType.MakeGenericType(type);
                }
                else
                {
                    specializedGeneric = interfaceType;
                }
                if (specializedGeneric.IsAssignableFrom(type))
                {
                    {
                        if (!type.GetTypeInfo().IsInterface && !type.GetTypeInfo().IsAbstract &&
                            !type.IsArray && !type.GetTypeInfo().IsValueType &&
                            !type.IsPointer && !typeof(Delegate).IsAssignableFrom(type))
                        {
                            typesToCheck.Add(type);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Checks this instance.
        /// </summary>
        public void Check()
        {
            foreach (var typeToCheck in typesToCheck)
            {
                var type = concreteGenericType.MakeGenericType(typeToCheck);
                if (!type.GetTypeInfo().ContainsGenericParameters)
                {
                    var checker = (IInterfaceChecker) Activator.CreateInstance(type);
                    checker.CheckInterface();
                }
            }
        }

        /// <summary>
        ///     Checks this instance.
        /// </summary>
        public void Check(Type[] types)
        {
            foreach (var tmpType in typesToCheck)
            {
                var typeToCheck = tmpType;
                if (typeToCheck.GetTypeInfo().ContainsGenericParameters)
                {
                    typeToCheck = typeToCheck.MakeGenericType(types);
                }
                var type = concreteGenericType.MakeGenericType(typeToCheck);
                if (!type.GetTypeInfo().ContainsGenericParameters)
                {
                    var checker = (IInterfaceChecker) Activator.CreateInstance(type);
                    checker.CheckInterface();
                }
            }
        }
    }
}