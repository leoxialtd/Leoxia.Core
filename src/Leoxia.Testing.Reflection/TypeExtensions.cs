#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeExtensions.cs" company="Leoxia Ltd">
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
using System.Linq;
using System.Reflection;

#endregion

namespace Leoxia.Testing.Reflection
{
    public static class TypeExtensions
    {
        private static readonly Random _random = new Random(Environment.TickCount);

        private static readonly BindingFlags _fieldFlags =
            BindingFlags.FlattenHierarchy |
            BindingFlags.Static |
            BindingFlags.NonPublic |
            BindingFlags.Public;

        public static bool IsInteger(this Type type)
        {
            return type == typeof(int) || type == typeof(long);
        }

        public static bool IsFloating(this Type type)
        {
            return type == typeof(decimal) || type == typeof(double) || type == typeof(float);
        }

        public static bool HasAttribute(this Type type, Type attributeType)
        {
            return type.GetTypeInfo().GetCustomAttribute(attributeType) != null;
        }

        public static object CreateRandomizedInstance(Type type)
        {
            if (type.IsInteger())
            {
                return _random.Next();
            }
            if (type.IsFloating())
            {
                return _random.NextDouble();
            }
            var instance = ObjectBuilder.CreateInstance(type, Environment.TickCount, true);
            foreach (var property in type.GetTypeInfo().GetProperties())
            {
                property.SetValue(instance, CreateRandomizedInstance(property.PropertyType));
            }
            return instance;
        }

        public static bool SetStaticField<T>(this Type type, string fieldName, T value)
        {
            var fields = type.GetTypeInfo().GetFields(_fieldFlags);
            var fieldInfo = fields.FirstOrDefault(x => x.Name == fieldName);
            if (fieldInfo == null)
            {
                return false;
            }
            fieldInfo.SetValue(null, value);
            return true;
        }

        public static PropertyInfo[] GetProperties(this Type type)
        {
            return type.GetTypeInfo().GetProperties();
        }

        public static Type[] GetGenericArguments(this Type type)
        {
            return type.GetTypeInfo().GetGenericArguments();
        }

        public static MethodInfo GetMethod(this Type type, string methodName, BindingFlags flags)
        {
            return type.GetTypeInfo().GetMethod(methodName, flags);
        }

        public static ConstructorInfo GetConstructor(this Type type, Type[] types)
        {
            return type.GetTypeInfo().GetConstructor(types);
        }

        public static PropertyInfo[] GetProperties(this Type type, BindingFlags flags)
        {
            return type.GetTypeInfo().GetProperties(flags);
        }

        public static PropertyInfo GetProperty(this Type type, string propertyName)
        {
            return type.GetTypeInfo().GetProperty(propertyName);
        }

        public static bool IsAssignableFrom(this Type type, Type other)
        {
            return type.GetTypeInfo().IsAssignableFrom(other);
        }

        public static ConstructorInfo[] GetConstructors(this Type type)
        {
            return type.GetTypeInfo().GetConstructors();
        }
    }
}