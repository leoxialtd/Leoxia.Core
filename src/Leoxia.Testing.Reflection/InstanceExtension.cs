#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceExtension.cs" company="Leoxia Ltd">
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
    ///     Extension methods for instance of objects.
    /// </summary>
    public static class InstanceExtensions
    {
        private static readonly BindingFlags _fieldFlags =
            BindingFlags.FlattenHierarchy |
            BindingFlags.Instance |
            BindingFlags.NonPublic |
            BindingFlags.Public;

        /// <summary>
        ///     Gets a new instance with the same type as specified object.
        /// </summary>
        /// <typeparam name="T">type of the new instance</typeparam>
        /// <param name="toCopy">To copy.</param>
        /// <returns>new instance</returns>
        public static T NewInstance<T>(this T toCopy)
        {
            if (toCopy != null)
            {
                var type = toCopy.GetType();
                return (T) NewInstance(type);
            }
            return Activator.CreateInstance<T>();
        }

        /// <summary>
        ///     Gets a new instance.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>new instance</returns>
        public static object NewInstance(Type type)
        {
            return Activator.CreateInstance(type);
        }

        /// <summary>
        ///     Randomizes the specified instance.
        /// </summary>
        /// <typeparam name="T">type of the instance</typeparam>
        /// <param name="toRandomize">To randomize.</param>
        /// <returns>randomized instance</returns>
        public static T Randomize<T>(T toRandomize)
        {
            Type type;
            if (toRandomize != null)
            {
                type = toRandomize.GetType();
            }
            else
            {
                type = typeof(T);
            }
            return (T) TypeExtensions.CreateRandomizedInstance(type);
        }

        /// <summary>
        ///     Sets the field value on the specified field name for the given object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="current">The current.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="System.ArgumentException">No field found. - fieldName</exception>
        public static void SetField<T>(this object current, string fieldName, T value)
        {
            var fieldInfo = GetFieldInfo(current, x => x.Name == fieldName);
            if (fieldInfo == null)
            {
                throw new ArgumentException("No field found.", nameof(fieldName));
            }
            fieldInfo.SetValue(current, value);
        }

        private static FieldInfo GetFieldInfo(object current, Func<FieldInfo, bool> predicate)
        {
            var fields = current.GetType().GetTypeInfo().GetFields(_fieldFlags);
            var fieldInfo = fields.FirstOrDefault(predicate);
            return fieldInfo;
        }

        /// <summary>
        ///     Sets the value for the first field in the given instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="current">The current.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="System.ArgumentException"></exception>
        public static void SetFirstField<T>(this object current, T value)
        {
            var fieldInfo = GetFieldInfo(current, x => x.FieldType == typeof(T));
            if (fieldInfo == null)
            {
                throw new ArgumentException($"No field of type {typeof(T)} found.");
            }
            fieldInfo.SetValue(current, value);
        }

        /// <summary>
        ///     Invokes the equality operator.
        /// </summary>
        /// <typeparam name="TInstance">The type of the instance.</typeparam>
        /// <param name="instance1">The instance1.</param>
        /// <param name="instance2">The instance2.</param>
        /// <returns></returns>
        internal static bool IsOperatorEqual<TInstance>(this TInstance instance1, TInstance instance2)
        {
            var info = typeof(TInstance).GetMethod("op_Equality",
                BindingFlags.Static | BindingFlags.Public |
                BindingFlags.InvokeMethod);
            if (info != null)
            {
                return (bool) info.Invoke(instance1, new object[] {instance1, instance2});
            }
            return false;
        }

        /// <summary>
        ///     Invokes the op inequality operator.
        /// </summary>
        /// <typeparam name="TInstance">The type of the instance.</typeparam>
        /// <param name="instance1">The instance1.</param>
        /// <param name="instance2">The instance2.</param>
        /// <returns></returns>
        internal static bool IsOperatorNotEqual<TInstance>(this TInstance instance1, TInstance instance2)
        {
            var info = typeof(TInstance).GetMethod("op_Inequality",
                BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod);
            return (bool) info.Invoke(instance1, new object[] {instance1, instance2});
        }
    }
}