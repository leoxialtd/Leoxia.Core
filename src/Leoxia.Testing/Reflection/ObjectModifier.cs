#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectModifier.cs" company="Leoxia Ltd">
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
    public static class ObjectModifier
    {
        /// <summary>
        ///     Changes the first property value of the given container.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="containerToChange">The container to change.</param>
        /// <returns></returns>
        public static bool ChangeFirstProperty<T>(T containerToChange)
        {
            var property = typeof(T).GetProperties().FirstOrDefault(p => p.CanRead && p.CanWrite);
            return ChangeValue(containerToChange, property.Name, true);
        }

        /// <summary>
        ///     Changes the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="containerToChange">The container to change.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static bool ChangeValue<T>(T containerToChange, string propertyName)
        {
            return ChangeValue(containerToChange, propertyName, false);
        }

        /// <summary>
        ///     Changes the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="containerToChange">The container to change.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="changeNonPublic">if set to <c>true</c> [change non public].</param>
        /// <returns></returns>
        internal static bool ChangeValue<T>(T containerToChange, string propertyName, bool changeNonPublic)
        {
            var type = typeof(T);
            var info = type.GetProperty(propertyName);
            var returnType = info.GetGetMethod(false).ReturnType;
            var oldValue = info.GetGetMethod(false).Invoke(containerToChange, new object[] { });
            object newValue = null;
            if (returnType.GetTypeInfo().IsPrimitive)
            {
                if (returnType == typeof(bool))
                {
                    newValue = !(bool) oldValue;
                }
                else if (returnType == typeof(long))
                {
                    newValue = (long) oldValue + 1;
                }
                else
                {
                    newValue = (int) oldValue + 1;
                }
            }
            else if (returnType == typeof(decimal))
            {
                newValue = (decimal) oldValue + 1;
            }
            else if (!returnType.GetTypeInfo().IsValueType)
            {
                if (returnType == typeof(string))
                {
                    if (oldValue == null)
                    {
                        oldValue = "";
                    }
                    newValue = (string) oldValue + "New Value";
                }
                else
                {
                    if (!returnType.GetTypeInfo().IsInterface && !returnType.GetTypeInfo().IsAbstract)
                    {
                        newValue = ObjectBuilder.CreateInstance(returnType, Environment.TickCount, true);
                    }
                }
            }
            if (newValue == null && oldValue == null)
            {
                return false;
            }
            var setMethod = info.GetSetMethod(changeNonPublic);
            if (setMethod != null)
            {
                setMethod.Invoke(containerToChange, new[] {newValue});
                return true;
            }
            return false;
        }
    }
}