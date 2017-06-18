#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectComparer.cs" company="Leoxia Ltd">
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
    public static class ObjectComparer
    {
        public static bool ObjectsAreEqual(object tested, object expected, PropertiesComparisonOptions options,
            CheckingTrace trace)
        {
            if (expected != null)
            {
                var expectedType = expected.GetType();
                if (tested == null)
                {
                    trace.SetFailure($"Tested value is null but expected {expectedType}:{expected} is not null.");
                    return false;
                }
                var testedType = tested.GetType();
                if (testedType != expectedType)
                {
                    trace.SetFailure($"Tested type {testedType} is not the expected {expectedType}.");
                    return false;
                }
                if (expectedType.GetTypeInfo().IsValueType ||
                    expectedType == typeof(string) ||
                    expectedType == typeof(Type))
                {
                    if (!expected.Equals(tested))
                    {
                        trace.SetFailure($"Tested value {tested} is not the expected {expected}.");
                        return false;
                    }
                }
                else
                {
                    if (typeof(IList).IsAssignableFrom(expectedType))
                    {
                        return ListsAreEqual((IList) tested, (IList) expected, options, trace);
                    }
                    // Manage other types
                    return PropertiesAreEqual(tested, expected, trace, options);
                }
            }
            else
            {
                if (tested != null)
                {
                    var type = tested.GetType();
                    trace.SetFailure($"Tested {type}:{tested} is not null but expecting null.");
                    return false;
                }
            }
            return true;
        }

        public static bool PropertiesAreEqual(object tested, object expected, CheckingTrace trace,
            PropertiesComparisonOptions comparisonOptions)
        {
            var expectedType = expected.GetType();
            var testedType = tested.GetType();
            var expectedLength = expectedType.GetProperties().Length;
            var testedLength = testedType.GetProperties().Length;
            if (expectedLength != testedLength &&
                comparisonOptions.CheckForNumberOfProperties)
            {
                trace.SetFailure(
                    $"Tested {testedType}[{testedLength}] has not the same number of properties as expected {expectedType}[{expectedLength}]");
                return false;
            }
            trace.Push(expectedType);
            var collection = SelectProperties(expectedType.GetProperties(), comparisonOptions.ExcludedProperties);
            return PropertiesAreEqual(expected, tested, collection, comparisonOptions, trace);
        }

        internal static string[] SelectProperties(ICollection<PropertyInfo> properties,
            IEnumerable<string> excludedList)
        {
            var names = new List<string>(properties.Count);
            foreach (var propertyInfo in properties)
            {
                if (propertyInfo.CanRead)
                {
                    var bAbbProperty = true;
                    if (excludedList != null)
                    {
                        foreach (var excludedProperty in excludedList)
                        {
                            if (excludedProperty == propertyInfo.Name)
                            {
                                bAbbProperty = false;
                                break;
                            }
                        }
                    }
                    if (bAbbProperty)
                    {
                        names.Add(propertyInfo.Name);
                    }
                }
            }
            return names.ToArray();
        }

        private static bool PropertiesAreEqual(object expected, object tested, IEnumerable<string> properties,
            PropertiesComparisonOptions comparisonOptions, CheckingTrace trace)
        {
            foreach (var propertyName in properties)
            {
                if (propertyName == "SyncRoot")
                {
                    continue;
                }
                var expectedType = expected.GetType();
                // get property infos
                var expectedProperty = expectedType.GetProperty(propertyName);
                var getter = expectedProperty.GetGetMethod();
                if (getter == null || getter.IsStatic)
                {
                    // no getter, or static
                    continue;
                }
                var testedType = tested.GetType();
                var testedProperty = testedType.GetProperty(expectedProperty.Name);
                if (testedProperty == null)
                {
                    trace.SetFailure(
                        $"{testedType}:{tested} has not the expected property {expectedProperty.Name} of {expectedType}");
                    return false;
                }
                // compare values
                if (expectedProperty.CanRead && testedProperty.CanRead)
                {
                    if (expectedProperty.GetIndexParameters().Length != 0)
                    {
                        continue;
                    }
                    var expectedValue = expectedProperty.GetValue(expected, null);
                    var testedValue = testedProperty.GetValue(tested, null);
                    trace.PushProperty(expectedType, expectedProperty.PropertyType, expectedProperty.Name);
                    if (!ObjectsAreEqual(expectedValue, testedValue, comparisonOptions, trace))
                    {
                        return false;
                    }
                    trace.Pop();
                }
            }
            return true;
        }

        private static bool ListsAreEqual(IList tested, IList expected, PropertiesComparisonOptions options,
            CheckingTrace trace)
        {
            if (expected != null)
            {
                if (tested == null)
                {
                    trace.SetFailure($"Tested list is null but expected list {expected.ShortDisplay()} is not null");
                    return false;
                }
                if (tested.Count != expected.Count)
                {
                    trace.SetFailure(
                        $"Tested list {tested.ShortDisplay()} has not same number of element that expected list {expected.ShortDisplay()}");
                    return false;
                }
                for (var i = 0; i < expected.Count; ++i)
                {
                    trace.PushIndex(expected.GetType(), i);
                    if (!ObjectsAreEqual(expected[i], tested[i], options, trace))
                    {
                        return false;
                    }
                    trace.Pop();
                }
                return true;
            }
            if (tested != null)
            {
                trace.SetFailure($"Tested list {tested.ShortDisplay()} is not null but expecting null list");
                return false;
            }
            return true;
        }
    }
}