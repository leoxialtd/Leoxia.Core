#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectFiller.cs" company="Leoxia Ltd">
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
using System.Reflection;

#endregion

namespace Leoxia.Testing.Reflection
{
    /// <summary>
    ///     Filler for objects
    /// </summary>
    public static class ObjectFiller
    {
        /// <summary>
        ///     Automatically affect values to all public properties
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="recurse">if set to <c>true</c> [b recurse].</param>
        /// <returns></returns>
        // ReSharper disable once ExcessiveIndentation
        public static bool Fill(object target, bool recurse)
        {
            var type = target.GetType();
            if (type.GetTypeInfo().IsValueType)
            {
                return false;
            }
            if (typeof(IList).IsAssignableFrom(type))
            {
                var tmp = (IList) ObjectBuilder.CreateIList(target.GetType(), 0, recurse);
                var list = target as IList;
                if (list != null)
                {
                    for (var i = 0; i < Math.Min(tmp.Count, list.Count); ++i)
                    {
                        if (list.IsFixedSize)
                        {
                            list[i] = tmp[i];
                        }
                        else
                        {
                            list.Add(tmp[i]);
                        }
                    }
                    return true;
                }
                return false;
            }
            return Fill(target, recurse, target.GetType().GetProperties());
        }

        /// <summary>
        ///     Automatically affect values to all public properties
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="bRecurse">if set to <c>true</c> [b recurse].</param>
        /// <param name="flags">The flags.</param>
        /// <returns></returns>
        public static bool Fill(object target, bool bRecurse, BindingFlags flags)
        {
            return Fill(target, bRecurse, target.GetType().GetProperties(flags));
        }

        /// <summary>
        ///     Automatically affect values to all public properties
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="bRecurse">True to recurse through member object</param>
        /// <param name="propertiesInfo">The properties info.</param>
        /// <returns></returns>
        // ReSharper disable once ExcessiveIndentation
        public static bool Fill(object target, bool bRecurse, PropertyInfo[] propertiesInfo)
        {
            foreach (var propertyInfo in propertiesInfo)
            {
                if (propertyInfo.CanRead && propertyInfo.CanWrite)
                {
                    if (propertyInfo.GetIndexParameters().Length == 0)
                    {
                        if (!propertyInfo.PropertyType.GetTypeInfo().IsInterface &&
                            !propertyInfo.PropertyType.GetTypeInfo().IsAbstract)
                        {
                            var newValue = ObjectBuilder.CreateInstance(propertyInfo.PropertyType,
                                propertiesInfo.Length, bRecurse);

                            if (newValue != null)
                            {
                                propertyInfo.SetValue(target, newValue, null);
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}