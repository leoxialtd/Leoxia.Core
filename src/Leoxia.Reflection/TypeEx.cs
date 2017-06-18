#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeEx.cs" company="Leoxia Ltd">
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
using System.Linq;
using System.Reflection;

#endregion

namespace Leoxia.Reflection
{
    public static class TypeEx
    {
        private static readonly Random _random = new Random(Environment.TickCount);

        private static readonly IDictionary<Type, object> defaultValues =
            new Dictionary<Type, object>();

        private static readonly TypeInfo listTypeInfo = typeof(IList).GetTypeInfo();
        private static readonly TypeInfo collectionTypeInfo = typeof(ICollection).GetTypeInfo();
        private static readonly Type listGenType = typeof(IList<>);
        private static readonly Type collectionGenType = typeof(ICollection<>);

        public static bool TryGetCollectionElement(this Type type, out Type elementType)
        {
            elementType = null;
            if (type.IsArray)
            {
                if (type.HasElementType)
                {
                    elementType = type.GetElementType();
                    return true;
                }
            }
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsGenericType)
            {
                if (listTypeInfo.IsAssignableFrom(typeInfo) ||
                    listGenType == type.GetGenericTypeDefinition() ||
                    collectionTypeInfo.IsAssignableFrom(typeInfo) ||
                    collectionGenType == type.GetGenericTypeDefinition())
                {
                    elementType = typeInfo.GetGenericArguments().First();
                    return true;
                }
            }
            return false;
        }


        public static object GetDefaultValue(this Type type)
        {
            object result;
            if (!defaultValues.TryGetValue(type, out result))
            {
                var genericType = typeof(DefaultProvider<>).MakeGenericType(type);
                result = ((IDefaultProvider) Activator.CreateInstance(genericType)).Default;
                defaultValues[type] = result;
            }
            return result;
        }

        public static bool IsCollectionType(this Type type)
        {
            if (type.IsArray)
            {
                if (type.HasElementType)
                {
                    return true;
                }
            }
            if (type.GetTypeInfo().IsGenericType)
            {
                if (listTypeInfo.IsAssignableFrom(type) ||
                    listGenType == type.GetGenericTypeDefinition() ||
                    collectionTypeInfo.IsAssignableFrom(type) ||
                    typeof(ICollection<>) == type.GetGenericTypeDefinition())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///     Gets the property value with the given name on the given instance of object.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static object GetPropertyValue(this object instance, string propertyName)
        {
            return instance?.GetType().GetTypeInfo().GetProperty(propertyName)?.GetValue(instance);
        }

        public static void SetPropertyValue(this object instance, string propertyName, object value)
        {
            instance?.GetType().GetTypeInfo().GetProperty(propertyName)?.SetValue(instance, value);
        }

        public static object GetRandomValue(this Type type)
        {
            if (type == typeof(int))
            {
                return _random.Next();
            }
            if (type == typeof(long))
            {
                return (long) _random.Next();
            }
            if (type == typeof(string))
            {
                return _random.Next().ToString();
            }
            return null;
        }

        /// <summary>
        ///     Always provide the valid (compilable) type name.
        ///     Useful for generics.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string GetValidName(this Type type)
        {
            var cached = CachedType.Get(type);
            return cached.Name;
        }
    }
}