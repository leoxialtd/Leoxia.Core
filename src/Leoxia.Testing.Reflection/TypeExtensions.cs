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
    /// <summary>
    ///     Extension method for <see cref="Type" />
    /// </summary>
    public static class TypeExtensions
    {
        private static readonly Random _random = new Random(Environment.TickCount);

        private static readonly BindingFlags _fieldFlags =
            BindingFlags.FlattenHierarchy |
            BindingFlags.Static |
            BindingFlags.NonPublic |
            BindingFlags.Public;

        /// <summary>
        ///     Determines whether this instance is integer.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///     <c>true</c> if the specified type is integer; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInteger(this Type type)
        {
            return type == typeof(int) || type == typeof(long);
        }

        /// <summary>
        ///     Determines whether this instance is floating.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///     <c>true</c> if the specified type is floating; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsFloating(this Type type)
        {
            return type == typeof(decimal) || type == typeof(double) || type == typeof(float);
        }

        /// <summary>
        ///     Determines whether the specified attribute type has attribute.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <returns>
        ///     <c>true</c> if the specified attribute type has attribute; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasAttribute(this Type type, Type attributeType)
        {
            return type.GetTypeInfo().GetCustomAttribute(attributeType) != null;
        }

        /// <summary>
        ///     Creates the randomized instance.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
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

        /// <summary>
        ///     Sets the static field.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
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

        /// <summary>
        ///     Gets the properties.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static PropertyInfo[] GetProperties(this Type type)
        {
            return type.GetTypeInfo().GetProperties();
        }

        /// <summary>
        ///     Gets the generic arguments.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static Type[] GetGenericArguments(this Type type)
        {
            return type.GetTypeInfo().GetGenericArguments();
        }

        /// <summary>
        ///     Gets the method.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="flags">The flags.</param>
        /// <returns></returns>
        public static MethodInfo GetMethod(this Type type, string methodName, BindingFlags flags)
        {
            return type.GetTypeInfo().GetMethod(methodName, flags);
        }

        /// <summary>
        ///     Gets the constructor.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="types">The types.</param>
        /// <returns></returns>
        public static ConstructorInfo GetConstructor(this Type type, Type[] types)
        {
            return type.GetTypeInfo().GetConstructor(types);
        }

        /// <summary>
        ///     Gets the properties.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="flags">The flags.</param>
        /// <returns></returns>
        public static PropertyInfo[] GetProperties(this Type type, BindingFlags flags)
        {
            return type.GetTypeInfo().GetProperties(flags);
        }

        /// <summary>
        ///     Gets the property.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static PropertyInfo GetProperty(this Type type, string propertyName)
        {
            return type.GetTypeInfo().GetProperty(propertyName);
        }

        /// <summary>
        ///     Determines whether [is assignable from] [the specified other].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="other">The other.</param>
        /// <returns>
        ///     <c>true</c> if [is assignable from] [the specified other]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAssignableFrom(this Type type, Type other)
        {
            return type.GetTypeInfo().IsAssignableFrom(other);
        }

        /// <summary>
        ///     Gets the constructors.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static ConstructorInfo[] GetConstructors(this Type type)
        {
            return type.GetTypeInfo().GetConstructors();
        }
    }
}