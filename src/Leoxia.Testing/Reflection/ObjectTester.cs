#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectTester.cs" company="Leoxia Ltd">
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
using System.Reflection;

#endregion

namespace Leoxia.Testing.Reflection
{
    public static class ObjectTester
    {
        private static T Convert<T>(object value)
        {
            return (T) value;
        }

        internal static bool IsInitialized<T>(this T instance)
        {
            if (!typeof(T).GetTypeInfo().IsValueType)
            {
                object o = instance;
                if (o == null)
                {
                    return false;
                }
            }
            if (!instance.Equals(default(T)))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Determines whether the specified to check is filled.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tested">To check.</param>
        /// <param name="trace">The trace.</param>
        /// <param name="options">The options.</param>
        /// <returns>
        ///     <c>true</c> if the specified to check is filled; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsFilled<T>(T tested, CheckingTrace trace, PropertiesComparisonOptions options)
        {
            if (tested != null)
            {
                var testedType = tested.GetType();
                if (testedType.GetTypeInfo().IsValueType)
                {
                    if (testedType == typeof(bool))
                    {
                        if ((bool) (object) tested == false)
                        {
                            trace.SetFailure("Tested is false but expected is true.");
                            return false;
                        }
                        return true;
                    }
                    if (testedType == typeof(TimeSpan))
                    {
                        decimal val = Convert<TimeSpan>(tested).Ticks;
                        if (val == 0)
                        {
                            trace.SetFailure("Tested Timespan is uninitialized.");
                            return false;
                        }
                        return true;
                    }
                    if (!IsInitialized(tested))
                    {
                        trace.SetFailure($"Tested {testedType} is uninitialized.");
                        return false;
                    }
                    return true;
                }
                if (testedType == typeof(string))
                {
                    var value = Convert<string>(tested);
                    if (value.Length == 0)
                    {
                        trace.SetFailure($"Tested string is empty.");
                        return false;
                    }
                }
                else
                {
                    PropertiesAreInitialized(tested, trace, options);
                }
            }
            else
            {
                trace.SetFailure($"Checked {typeof(T)} is null");
                return false;
            }
            return true;
        }

        public static bool PropertiesAreInitialized<T>(T propertiesContainer, CheckingTrace trace,
            PropertiesComparisonOptions options)
        {
            if (propertiesContainer == null)
            {
                throw new ArgumentNullException(nameof(propertiesContainer),
                    "Cannot check if properties are initialized on null instance");
            }
            var type = propertiesContainer.GetType();
            var properties = ObjectComparer.SelectProperties(type.GetProperties(), options.ExcludedProperties);
            foreach (var propertyName in properties)
            {
                var property = type.GetProperty(propertyName);
                if (options.IgnoreReadOnlyProperties && !property.CanWrite)
                {
                    continue;
                }
                if (property.CanRead)
                {
                    // Indexed Properties are managed
                    if (property.GetIndexParameters().Length == 0)
                    {
                        var value = property.GetValue(propertiesContainer, null);
                        trace.PushProperty(type, property.PropertyType, propertyName);
                        if (!IsFilled(value, trace, options))
                        {
                            return false;
                        }
                        trace.Pop();
                    }
                }
            }
            return true;
        }
    }
}