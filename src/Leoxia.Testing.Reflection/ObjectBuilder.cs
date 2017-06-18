#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectBuilder.cs" company="Leoxia Ltd">
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
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

#endregion

namespace Leoxia.Testing.Reflection
{
    /// <summary>
    ///     Builder for objects
    /// </summary>
    public static class ObjectBuilder
    {
        private static readonly object[] dummyParams = { };

        /// <summary>
        ///     Call MemberwiseClone on given object.
        /// </summary>
        /// <param name="instanceToClone">The instance to clone.</param>
        /// <returns></returns>
        public static object MemberwiseClone(object instanceToClone)
        {
            var methodInfo = instanceToClone.GetType().GetMethod("MemberwiseClone",
                BindingFlags.NonPublic |
                BindingFlags.Instance |
                BindingFlags.InvokeMethod |
                BindingFlags.FlattenHierarchy);
            return methodInfo.Invoke(instanceToClone, dummyParams);
        }

        internal static object CreateIList(Type type, int seed, bool recurse)
        {
            //ConstructorInfo constructorInfo = type.GetConstructor(BindingFlags.Static|BindingFlags.Public |BindingFlags.CreateInstance, null, new Type[]{typeof(int)}, new ParameterModifier[]{});
            var args = new object[] {3};
            var newValue = Activator.CreateInstance(type, args);
            for (var i = 0; i < 3; ++i)
            {
                object resultValue;
                Type elementType;
                if (type.HasElementType)
                {
                    elementType = type.GetElementType();
                }
                else
                {
                    elementType = type.GetGenericArguments()[0];
                }
                var flag = CreateValue(elementType, seed, out resultValue);
                if (!flag)
                {
                    if (elementType == typeof(string))
                    {
                        resultValue = seed.ToString();
                    }
                    else
                    {
                        var elementInfo = elementType.GetConstructor(Type.EmptyTypes);
                        if (elementInfo != null)
                        {
                            resultValue = elementInfo.Invoke(null);
                            if (recurse)
                            {
                                ObjectFiller.Fill(resultValue, true);
                            }
                        }
                    }
                }
                AddInList(type, i, newValue, resultValue);
            }
            return newValue;
        }

        private static void AddInList(Type type, int index, object list, object value)
        {
            if (type.IsArray)
            {
                ((IList) list)[index] = value;
            }
            else
            {
                ((IList) list).Add(value);
            }
        }

        public static object CreateInstance(Type type, int seed, bool recurse)
        {
            object resultValue;
            var flag = CreateValue(type, seed, out resultValue);
            if (flag)
            {
                return resultValue;
            }
            if (type == typeof(string))
            {
                return seed.ToString();
            }
            if (typeof(IList).IsAssignableFrom(type))
            {
                return CreateIList(type, seed, recurse);
            }
            var elementInfo = type.GetConstructor(Type.EmptyTypes);
            if (elementInfo != null)
            {
                resultValue = elementInfo.Invoke(null);
                if (recurse)
                {
                    ObjectFiller.Fill(resultValue, true);
                }
            }
            else
            {
                var constructorInfos = type.GetConstructors();
                if (constructorInfos.Length == 0)
                {
                    throw new InvalidOperationException($"Cannot find constructors for {type}");
                }
                elementInfo = constructorInfos[0];
                var parameters = new List<object>();
                foreach (var info in elementInfo.GetParameters())
                {
                    parameters.Add(CreateInstance(info.ParameterType, Environment.TickCount, false));
                }
                resultValue = elementInfo.Invoke(parameters.ToArray());
                if (recurse)
                {
                    ObjectFiller.Fill(resultValue, true);
                }
            }
            return resultValue;
        }

        /// <summary>
        ///     Mocks the value.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="seed">The seed used to generate a value.</param>
        /// <param name="newValue">The new value.</param>
        /// <returns></returns>
        internal static bool CreateValue(Type type, int seed, out object newValue)
        {
            newValue = null;
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsPrimitive)
            {
                if (type == typeof(bool))
                {
                    newValue = true;
                }
                else if (type == typeof(double))
                {
                    newValue = (double) seed;
                }
                else if (type == typeof(float))
                {
                    newValue = (float) seed;
                }
                else if (type == typeof(long))
                {
                    newValue = (long) seed;
                }
                else if (type == typeof(int))
                {
                    newValue = seed;
                }
                else if (type == typeof(byte))
                {
                    newValue = (byte) seed;
                }
                else if (type == typeof(char))
                {
                    newValue = (char) seed;
                }
                else
                {
                    return true;
                }
            }
            else if (typeInfo.IsValueType)
            {
                if (type == typeof(decimal))
                {
                    newValue = (decimal) seed;
                }
                else if (typeInfo.IsEnum)
                {
                    var names = Enum.GetNames(type);
                    // Special case for flags in order to check for limit cases
                    if (type.HasAttribute(typeof(FlagsAttribute))
                        && names.Length > 1)
                    {
                        var index = seed % (names.Length - 1);
                        newValue = (int) Enum.Parse(type, names[names.Length - 1]) +
                                   (int) Enum.Parse(type, names[index]);
                    }
                    else
                    {
                        var index = seed % names.Length;
                        newValue = Enum.Parse(type, names[index]);
                    }
                }
                else if (type == typeof(DateTime))
                {
                    newValue = DateTime.Now;
                    return true;
                }
                else if (type == typeof(TimeSpan))
                {
                    newValue = TimeSpan.FromMilliseconds(Environment.TickCount);
                    return true;
                }
                else if (type == typeof(string))
                {
                    newValue = seed.ToString();
                    return true;
                }
                return true;
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}